using bigbus.checkout.data.Model;
using pci = Common.Model.Pci;
using Basket = bigbus.checkout.data.Model.Basket;

namespace Services.Infrastructure
{
    public interface ICheckoutService
    {
        Order CreateOrder(Session session, Basket basket, pci.BasketStatus basketStatus, string clientIpAddress,
            string languageId);

        void SaveOrder(Order order);
    }
}
