using bigbus.checkout.data.Model;
using pci = Common.Model.Pci;
using Basket = bigbus.checkout.data.Model.Basket;
using System.Collections.Generic;

namespace Services.Infrastructure
{
    public interface ICheckoutService
    {
        Order GetFullOrder(string orderId);

        Order CreateOrder(Session session, Basket basket, pci.BasketStatus basketStatus, string clientIpAddress,
            string languageId, string microSiteId);

        Order CreateOrderPayPal(Session session, Basket basket, User user, string clientIpAddress, 
            string languageId, string microSiteId);

        void SaveOrder(Order order);

        TransactionAddressPaypal CreateAddressPaypal(Order order, Session session, User user);

        List<OrderLineGeneratedBarcode> GetOrderLineGeneratedBarcodes(OrderLine orderLine);

        void SaveOrderLineBarCode(OrderLineGeneratedBarcode orderLineGBC);

        List<DiallingCode> GetAlldiallingDiallingCodes();

        DiallingCode GetDiallingCode(string id);

        bool OrderAllTicketShowMobile(Order order);
    }
}
