
using System;
using System.Linq;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Model;
using pci = Common.Model.Pci;
using Services.Infrastructure;
using Basket = bigbus.checkout.data.Model.Basket;
using Common.Model.Ecr;
using System.Collections.Generic;

namespace Services.Implementation
{
    public class CheckoutService : BaseService, ICheckoutService
    {
        private readonly IGenericDataRepository<Order> _orderRepository;
        private readonly IGenericDataRepository<User> _userRepository;
        private readonly IGenericDataRepository<Ticket> _ticketRepository;
        private readonly IGenericDataRepository<Currency> _currencyRepository;
        private readonly IGenericDataRepository<TransactionAddressPaypal> _addressPpRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IGenericDataRepository<OrderLineGeneratedBarcode> _barcodeRepository;

        public CheckoutService(): base()
        {

        }

        public CheckoutService(IGenericDataRepository<Order> orderRepository, IGenericDataRepository<User> userRepository, IGenericDataRepository<Ticket> ticketRepository,
            IGenericDataRepository<Currency> currencyRepository, ILocalizationService localizationService, IGenericDataRepository<TransactionAddressPaypal> addressPPRepository,
            IGenericDataRepository<OrderLineGeneratedBarcode> barcodeRepos)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _currencyRepository = currencyRepository;
            _localizationService = localizationService;
            _addressPpRepository = addressPPRepository;
            _barcodeRepository = barcodeRepos;
            
        }

        public virtual Order CreateOrder(Session session, Basket basket, pci.BasketStatus basketStatus, string clientIpAddress, string languageId, string micrositeId)
        {
            try
            {
                Guid? currencyId =  new Guid(session.CurrencyId);
                var currency = _currencyRepository.GetSingle(x => x.Id.Equals(currencyId));
                var user = _userRepository.GetSingle(x => x.Id == basket.UserId);

                var order = new Order
                {
                    PaymentMethod = basketStatus.card.type,
                    NumbViewPdf = 0,
                    OpenForPrinting = true,
                    DatePdfLastViewed = DateTime.Now,
                    UserId = user.Id,
                    UserName = user.Firstname + " " + user.Lastname,
                    EmailAddress = string.IsNullOrWhiteSpace(user.FriendlyEmail) ? user.Email : user.FriendlyEmail,
                    CurrencyId = currencyId,
                    LanguageId = languageId,
                    Total = basket.Total,
                    CcLast4Digits = basketStatus.card.number,
                    ClientIp = clientIpAddress,
                    WorldPayMerchantId = basketStatus.merchant.merchantId,
                    GatewayReference = basketStatus.merchant.gatewayref,
                    NameOnCard = basketStatus.card.name,
                    BasketId = basket.Id,
                    SessionId = session.Id,
                    TotalQuantity = basket.BasketLines.Sum(x => x.TicketQuantity),
                    IsMobileAppOrder = false, //check how this is populated on old system.
                    DateCreated = _localizationService.GetLocalDateTime(micrositeId)
                };

                //populate order lines with existing baskets.
                foreach (var basketLine in basket.BasketLines)
                {
                    order.OrderLines.Add(ConvertBasketLineToOrderLine(basketLine, order.Id));
                }

                _orderRepository.Add(order);

                order.User = user;
                order.Currency = currency;

                return order;
            }
            catch (Exception ex)
            {
                Log(string.Format("Order Create Failed sessionid: {0} basketid:{1} {2} ", session.Id, basket.Id, ex.Message));
                return null;
            }
           
        }

        public virtual Order CreateOrderPayPal(Session session, Basket basket, User user, string clientIpAddress, string languageId, string micrositeId)
        {
            try
            {
                Guid? currencyId = new Guid(session.CurrencyId);
                var currency = _currencyRepository.GetSingle(x => x.Id.Equals(currencyId));
               
                var order = new Order
                {
                    PaymentMethod = "PayPal",
                    NumbViewPdf = 0,
                    OpenForPrinting = true,
                    DatePdfLastViewed = DateTime.Now,
                    UserId = user.Id,
                    UserName = user.Firstname + " " + user.Lastname,
                    EmailAddress = string.IsNullOrWhiteSpace(user.FriendlyEmail) ? user.Email : user.FriendlyEmail,
                    CurrencyId = currencyId,
                    LanguageId = languageId,
                    Total = basket.Total,
                    ClientIp = clientIpAddress,
                    BasketId = basket.Id,
                    SessionId = session.Id,
                    TotalQuantity = basket.BasketLines.Sum(x => x.TicketQuantity),
                    IsMobileAppOrder = false, //check how this is populated on old system.
                    DateCreated = _localizationService.GetLocalDateTime(micrositeId),
                    PayPalOrderId = session.PayPalOrderId,
                    PayPalPayerId = session.PayPalPayerId,
                    PayPalToken = session.PayPalToken
                };

                //populate order lines with existing baskets.
                foreach (var basketLine in basket.BasketLines)
                {
                    order.OrderLines.Add(ConvertBasketLineToOrderLine(basketLine, order.Id));
                }

                _orderRepository.Add(order);

                order.User = user;
                order.Currency = currency;

                return order;
            }
            catch (Exception ex)
            {
                Log(string.Format("Paypal Order Create Failed sessionid: {0} basketid:{1} {2} ", session.Id, basket.Id, ex.Message));
                return null;
            }  
        }

        public virtual TransactionAddressPaypal CreateAddressPaypal(Order order, Session session, User user)
        {
            var payPalAddress = new TransactionAddressPaypal
            {
                OrderId = order.Id.ToString(),
                OrderNumber = order.OrderNumber,
                Email = user.Email,
                Title = user.Title,
                BillToFirstName = user.Firstname,
                BillToLastName = user.Lastname,

                //Address broken up and combined to make street
                BillToAddress1 = user.AddressLine1,
                BillToAddress2 = user.AddressLine2,
                BillToStreet = string.Concat(user.AddressLine1, " ", user.AddressLine2),

                BillToCity = user.City,
                BillToPostCode = user.PostCode,
                BillToState = user.StateProvince,
                BillToCountry = user.CountryId,

                //Shipping to information
                ShipToFirstName = user.Firstname,
                ShipToLastName = user.Lastname,

                //Address broken up and combined to make street
                ShipToAddress1 = user.AddressLine1,
                ShipToAddress2 = user.AddressLine2,
                ShipToStreet = string.Concat(user.AddressLine1, " ", user.AddressLine2),

                ShipToCity = user.City,
                ShipToPostCode = user.PostCode,
                ShipToState = user.StateProvince,
                ShipToCountry = user.CountryId,

                SessionId = session.Id.ToString(),
                UserId = session.UserId.ToString(),
                PayPalOrderId = session.PayPalOrderId,
                PayPalToken = session.PayPalToken,
                PayPalPayerId = session.PayPalPayerId,
                BasketId = session.BasketId.ToString(),
            };

            _addressPpRepository.Add(payPalAddress);

            return payPalAddress;
        }

        public virtual OrderLine ConvertBasketLineToOrderLine(BasketLine basketLine, Guid orderId)
        {
            Log(string.Format("Entering CheckoutService - ConvertBasketLineToOrderLine() orderid:{0} - basketlineid {1}", orderId, basketLine.Id));

            var basketLineTicket = _ticketRepository.GetSingle(x => x.Id == basketLine.TicketId);

            var orderLine = new OrderLine
            {
                OrderId = orderId,
                TicketId = basketLine.TicketId,
                FixedDateTicket = basketLine.FixedDateTicket,
                TicketDate = basketLine.TicketDate,
                TicketType = basketLine.TicketType,
                TicketQuantity = basketLine.TicketQuantity,
                PromotionId = basketLine.PromotionId,
                DepartureTimeHour = basketLine.DepartureTimeHour,
                DepartureTimeMinute = basketLine.DepartureTimeMinute,
                DeparturePoint = basketLine.DeparturePoint,
                TicketTorA = basketLineTicket.TicketType,
                MicrositeId = basketLineTicket.MicroSiteId,
                TicketCost = basketLine.Price,
                GrossOrderLineValue = basketLine.LineTotal + basketLine.Discount,
                NettOrderLineValue = basketLine.LineTotal
            };

            if (basketLineTicket.TicketType.Equals("attraction", StringComparison.CurrentCultureIgnoreCase))
            {
                orderLine.AttractionName = basketLineTicket.Name;
            }

            return orderLine;
        }

        public virtual void SaveOrder(Order order)
        {
            _orderRepository.Update(order);
        }

        public virtual void SaveOrderLineBarCodes(BookingResult result, Order order)
        {
            var lines = order.OrderLines;
            var selectedLines = new List<string>();

            foreach (var code in result.Barcodes)
            {
                var orderline = lines.FirstOrDefault(x => x.TicketId.Value.ToString().Equals(code.TicketId, StringComparison.CurrentCultureIgnoreCase));
                selectedLines.Add(orderline.Id.ToString());

                _barcodeRepository.Add(new OrderLineGeneratedBarcode
                {
                     DateCreated = DateTime.Now,
                     OrderLineId = orderline.Id,
                     GeneratedBarcode = code.Code
                });
            }
        }

        public virtual Order GetFullOrder(string orderId)
        {
            var order = OrderRepository.GetSingle(x => x.Id.ToString().Equals(orderId, StringComparison.CurrentCultureIgnoreCase));

            order.OrderLines = OrderLineRepository.GetList(x => x.OrderId.ToString().Equals(orderId, StringComparison.CurrentCultureIgnoreCase));

            return order;
        }

    }
}
