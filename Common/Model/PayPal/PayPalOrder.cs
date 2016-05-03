using System.Collections.Generic;

namespace Common.Model.PayPal
{
    public class PayPalOrder
    {
        public bool RequestShipping { get; set; }
        public string ISOCurrencyCode { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal OrderSubTotal { get; set; }
        public decimal OrderTaxTotal { get; set; }
        public string orderLanguage { get; set; }
        public IList<PayPalOrderItem> Items { get; set; }
    }
}
