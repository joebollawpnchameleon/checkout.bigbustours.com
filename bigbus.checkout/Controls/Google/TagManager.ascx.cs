using bigbus.checkout.data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace bigbus.checkout.Controls.Google
{
    public partial class TagManager : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ecommerceTracking.Visible = false;

            // if we dont have an order to track exit
            if (string.IsNullOrWhiteSpace(OrderId)) return;

            if (Order == null)
                Order =BasePage.CheckoutService.GetFullOrder(OrderId);

            // todo: if we have and orderId which does not return a valid order then log it
            if (Order == null) return;

            // Start -> _addTrans
            TransactionId = Order.OrderNumber;
            TransactionAffiliation = string.Empty;
            TransactionTotal = Order.Total.Value;
            TransactionTax = 0;
            TransactionShipping = 0;
            TransactionCurrency = Order.Currency.ISOCode ;
            products.DataSource = Order.OrderLines;
            products.DataBind();

            ecommerceTracking.Visible = true;

        }       

        public Order Order { get; set; }

        public string OrderId { get; set; }

        public int TransactionId { get; set; }

        public string TransactionAffiliation { get; set; }
        public decimal TransactionTotal { get; set; }
        public decimal TransactionTax { get; set; }
        public decimal TransactionShipping { get; set; }

        public string TransactionCurrency { get; set; }
    }
}