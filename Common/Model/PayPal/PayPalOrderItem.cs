
namespace Common.Model.PayPal
{
    public class PayPalOrderItem
    {
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal LineItemPrice { get; set; }
        public decimal LineItemTax { get; set; }

        public PayPalOrderItem(string productName, string productId, int quantity, decimal lineItemPrice, decimal lineItemTax)
        {
            ProductName = productName;
            ProductId = productId;
            Quantity = quantity;
            LineItemPrice = lineItemPrice;
            LineItemTax = lineItemTax;
        }
    }
}
