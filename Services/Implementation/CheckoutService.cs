
using System;
using System.Linq;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Model;
using pci = Common.Model.Pci;
using Services.Infrastructure;
using Basket = bigbus.checkout.data.Model.Basket;

namespace Services.Implementation
{
    public class CheckoutService : BaseService, ICheckoutService
    {
        private readonly IGenericDataRepository<Order> _orderRepository;
        private readonly IGenericDataRepository<User> _userRepository;
        private readonly IGenericDataRepository<Ticket> _ticketRepository;
        private readonly IGenericDataRepository<Currency> _currencyRepository;

        public CheckoutService(IGenericDataRepository<Order> orderRepository, IGenericDataRepository<User> userRepository, IGenericDataRepository<Ticket> ticketRepository,
            IGenericDataRepository<Currency> currencyRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _currencyRepository = currencyRepository;
        }

        public virtual Order CreateOrder(Session session, Basket basket, pci.BasketStatus basketStatus, string clientIpAddress, string languageId)
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
                    DateCreated = DateTime.Now
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
                GrossOrderLineValue = basketLine.LineTotal,
                NettOrderLineValue = basketLine.LineTotal - basketLine.Discount
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
    }
}
