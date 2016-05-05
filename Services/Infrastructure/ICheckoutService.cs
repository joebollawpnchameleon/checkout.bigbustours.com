using bigbus.checkout.data.Model;
using pci = Common.Model.Pci;
using Basket = bigbus.checkout.data.Model.Basket;
using Common.Model.Ecr;

namespace Services.Infrastructure
{
    public interface ICheckoutService
    {
        Order CreateOrder(Session session, Basket basket, pci.BasketStatus basketStatus, string clientIpAddress,
            string languageId, string microSiteId);

        Order CreateOrderPayPal(Session session, Basket basket, User user, string clientIpAddress, 
            string languageId, string microSiteId);

        void SaveOrder(Order order);

        TransactionAddressPaypal CreateAddressPaypal(Order order, Session session, User user);

        void SaveOrderLineBarCodes(BookingResult result, Order order);
    }
}
