using bigbus.checkout.data.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace bigbus.checkout.Controls.Google
{
    public partial class TagManager : BaseControl
    {
        protected string BaseCurrencyCode
        {
            get { return ConfigurationManager.AppSettings["Default.GA.Currency"]; }
        }

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
            if (Order.Total != null) TransactionTotal = Order.Total.Value;
            TransactionTax = 0;
            TransactionShipping = 0;
            TransactionCurrency = Order.Currency.ISOCode ;
            //products.DataSource = Order.OrderLines;
            //products.DataBind();

            ecommerceTracking.Visible = true;

        }

        public string MakeOrderLines(List<OrderLine> orderLines)
        {
            var sbTemp = new StringBuilder();

            foreach (var orderline in orderLines)
            {
                var ticket = BasePage.TicketService.GetTicketById(orderline.TicketId.ToString());

                if (sbTemp.Length > 10)
                {
                    sbTemp.AppendLine(",");
                }

                sbTemp.AppendLine("{'name': '" + orderline.TicketType + " - " + ticket.Name.Replace("'", "\'") + "',");
                sbTemp.AppendLine("'id': '" + ticket.Name.Replace("'", "\'") +"',");
                sbTemp.AppendLine("'price': '" + ConvertPriceToBaseCurrency(orderline.TicketCost.Value) + "',");
                sbTemp.AppendLine("'brand': 'Google',");
                sbTemp.AppendLine("'category': '" + orderline.TicketTorA + "',");
                sbTemp.AppendLine("'variant': '',");
                sbTemp.AppendLine("'quantity': " + orderline.TicketQuantity + ",");
                sbTemp.AppendLine("'coupon': ''");//use coupon from BORN
                sbTemp.AppendLine(" }");
            }

            return sbTemp.ToString();
        }

        public string GetTagManagerScript()
        {
           
            try
            {
                return @"dataLayer.push({                                            
                    'ecommerce': {
                        'purchase': {
                            'actionField': {
                                'id': '" + TransactionId + "'," + Environment.NewLine +
                             "'affiliation': '" + TransactionAffiliation + "'," + Environment.NewLine +
                             "'revenue': '" + ConvertPriceToBaseCurrency(TransactionTotal) + "'," + Environment.NewLine +
                             "'tax':'" + ConvertPriceToBaseCurrency(TransactionTax) + "'," + Environment.NewLine +
                             "'shipping': '" + ConvertPriceToBaseCurrency(TransactionShipping) + "'," + Environment.NewLine +
                             "'coupon': ''" + Environment.NewLine +
                             @"},                    
                            'products': [" + MakeOrderLines(Order.OrderLines.ToList()) + @"]
                        }
                    }
                });";
            }
            catch (Exception ex)
            {
                BasePage.Log("TagManager Exception ex:" + ex.Message);
                return string.Empty;
            }
        }

        private decimal ConvertPriceToBaseCurrency(decimal price)
        {
            try
            {
                var baseCurrencyCode = ConfigurationManager.AppSettings["Default.GA.Currency"];
                if (TransactionCurrency.Equals(baseCurrencyCode, StringComparison.CurrentCultureIgnoreCase))
                    return price;

                var baseCurrencyConfigValue = Convert.ToDecimal(ConfigurationManager.AppSettings["GA." + baseCurrencyCode]);
                var currencyConfigValue = Convert.ToDecimal(ConfigurationManager.AppSettings["GA." + TransactionCurrency]);

                var convertedPrice = (baseCurrencyConfigValue*price)/ currencyConfigValue;

                return Math.Round(convertedPrice, 2);
            }
            catch (Exception ex)
            {
                BasePage.Log("ConvertPriceToBaseCurrency() breaks for price: " + price);
                return price;
            }

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