using System;
using System.Collections.Generic;
using System.Linq;
using bigbus.checkout.data.Model;
using Common.Model;
using Services.Infrastructure;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Model.PayPal;
using pci = Common.Model.Pci;

namespace Services.Implementation
{
    public class BasketService : IBasketService
    {
        private readonly IGenericDataRepository<Basket> _repository;
        private readonly IGenericDataRepository<BasketLine> _lineRepository;
        private readonly ICurrencyService _currencyService;
        private readonly ITicketService _ticketService;
        private readonly ISiteService _siteService;
        private readonly ITranslationService _translationService;

        public BasketService(IGenericDataRepository<Basket> repository, 
            IGenericDataRepository<BasketLine> lineRepository,
            ICurrencyService currencyService, 
            ITicketService ticketService, 
            ISiteService siteService,
            ITranslationService translationService)
        {
            _repository = repository;
            _lineRepository = lineRepository;
            _currencyService = currencyService;
            _ticketService = ticketService;
            _siteService = siteService;
            _translationService = translationService;
        }

        public virtual Basket GetBasket(Guid basketId)
        {
            return _repository.GetSingle(x => x.Id == basketId, x => x.BasketLines);
        }

        public virtual bool IsBornBasketValid(BornBasket brnBasket)
        {
            var currencyId = _currencyService.GetCurrencyGuidByCode(brnBasket.CurrencyCode);
            
            //we cannot persist a basket without that external guid
            if (string.IsNullOrEmpty(brnBasket.ExternalCookieValue))
            {
                return false;
            }

            //validate currency in basket
            if (currencyId == Guid.Empty)
            {
                //log issue and return false; 
                return false;
            }

            brnBasket.CurrencyId = currencyId;

            //validate all tickets in basket
            foreach (var basketItem in brnBasket.BasketItems)
            {
                var ticket = _ticketService.GetTicketBySku(basketItem.Sku);
                if (ticket == null)
                {
                    //log invalid ticket and return
                    return false;
                }
                basketItem.TicketId = ticket.Id;
            }

            return true;
        }

        public virtual bool DoesBasketExist(string externalSessionId)
        {
            var basket = _repository.GetSingle(
                x => !string.IsNullOrEmpty(x.ExternalCookieValue) && 
                    externalSessionId.Trim().Equals(x.ExternalCookieValue.Trim(), StringComparison.CurrentCultureIgnoreCase));

            return basket != null;
        }

        public virtual Guid PersistBasket(BornBasket brnBasket)
        {
            //make sure basket is valid before persisting it.
            if(!IsBornBasketValid(brnBasket))
                return Guid.Empty;

            var basket = new Basket
            {
                DateCreated = DateTime.Now,
                CurrencyId = brnBasket.CurrencyId,
                Total = brnBasket.Total,
                DiscountValue = brnBasket.Discount,
                ExternalCookieValue = brnBasket.ExternalCookieValue,
                ExternalCoupon = brnBasket.Coupon,
                PurchaseLanguage = brnBasket.Language
            };
            
            foreach (var item in brnBasket.BasketItems)
            {
                basket.BasketLines.Add(
                    new BasketLine
                    {
                        TicketId = item.TicketId,
                        FixedDateTicket = false,
                        TicketType = item.TicketType.ToString(),
                        TicketQuantity = item.Quantity,
                        DateAdded = DateTime.Now,
                        Price = item.UnitCost,
                        Discount = item.Discount,
                        LineTotal = item.Total
                    });
                    
            }
           
            _repository.Add(basket);

            return basket.Id;
        }

        public virtual Basket GetBasketBySessionId(string externalSessionId)
        {
            var basket =
                _repository.GetSingle(
                    x =>
                        x.ExternalCookieValue != null &&
                        x.ExternalCookieValue.Trim().Equals(externalSessionId.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (basket != null && (basket.BasketLines == null || basket.BasketLines.Count == 0))
            {
                basket.BasketLines = _lineRepository.GetList(x => x.BasketId == basket.Id);
            }

            return basket;
        }

        public virtual bool ConnectUserToBasket(Guid userId, Guid basketId)
        {
            var basket = _repository.GetSingle(x => x.Id == basketId);
            
            if (basket == null)
                return false;

            basket.UserId = userId;
            _repository.Update(basket);

            return true;
        }

        public virtual pci.Basket GetPciBasket(Customer customer, Basket basket)
        {
            var pciBasket =
                new pci.Basket
                {
                    ID = basket.Id.ToString(),
                    IsAgent = false,
                    ISOCurrencyCode = _currencyService.GetCurrencyIsoCodeById(basket.CurrencyId.ToString()), 
                    IsFromUS = _siteService.GetMicroSiteById(customer.MicroSiteId).IsUS , 
                    Total = basket.Total
                };

            var itemsToAdd = new List<pci.BasketLine>();

            foreach (var basketLine in basket.BasketLines)
            {
                var item = new pci.BasketLine
                {
                    Name = _ticketService.GetTicketById(basketLine.TicketId.ToString()).Name, //***get ticket name from related ticket
                    TicketQuantity = basketLine.TicketQuantity ?? 0,
                    TicketType = basketLine.TicketType,
                    Total = (basketLine.LineTotal != null)? basketLine.LineTotal.Value : (decimal) 0.0,
                    Date = DateTime.Now
                };

                if (basketLine.FixedDateTicket != null && basketLine.FixedDateTicket.Value)
                {
                    item.Date = basketLine.TicketDate;
                }

                itemsToAdd.Add(item);
            }

            pciBasket.Items = itemsToAdd.ToArray();

            string email;

            if (!string.IsNullOrWhiteSpace(customer.FriendlyEmail))
            {
                email = customer.FriendlyEmail;
            }
            else
            {
                email =
                    customer.Email.Trim().StartsWith(customer.Id + "_")
                        ? customer.Email.Trim().Replace(customer.Id + "_", string.Empty)
                        : customer.Email.Trim();
            }

            var user =
                new pci.User
                {
                    FirstName = customer.Firstname,
                    LastName = customer.Lastname,
                    AddressLine1 = customer.AddressLine1,
                    City = customer.City,
                    PostCode = customer.PostCode,
                    CountryCode = customer.CountryId,
                    EmailAddress = email,
                    FullName = customer.Firstname + " " + customer.Lastname
                };

            pciBasket.User = user;

            return pciBasket;
        }

        public virtual PayPalOrder BuildPayPalOrder(Basket basket)
        {
            var basketLine = basket.BasketLines.FirstOrDefault();

            if (basketLine == null)
                return null;

            var ticket = _ticketService.GetTicketById(basketLine.TicketId.ToString());
            var currencyIsoCode = _currencyService.GetCurrencyIsoCodeById(basket.CurrencyId.ToString());
            var language = _translationService.GetLanguage(basket.PurchaseLanguage);

            var orderItem = new PayPalOrderItem(
                    ticket.Name,
                    basketLine.Id.ToString(),
                    1,
                    basket.Total,
                    0);

            var order = new PayPalOrder()
            {
                Items = new List<PayPalOrderItem>() { orderItem },
                ISOCurrencyCode = currencyIsoCode,
                OrderSubTotal = basket.Total,
                OrderTaxTotal = 0,
                OrderTotal = basket.Total,
                RequestShipping = true,
                orderLanguage = language.ShortCode
            };

            return order;
        }

        public virtual Basket GetLatestBasket()
        {
            var all = _repository.GetList(x => !string.IsNullOrEmpty(x.ExternalCookieValue));
            var topBasket = all.OrderByDescending(x => x.DateCreated);
            return topBasket.FirstOrDefault();
        }
    }
}
