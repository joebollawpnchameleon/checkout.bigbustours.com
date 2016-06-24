using bigbus.checkout.data.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using Services.Implementation;

namespace bigbus.checkout.Controls.Google
{
    public partial class TagManager : BaseControl
    {
        private string _baseCurrencyCode;

        protected string BaseCurrencyCode
        {
            get { return string.IsNullOrEmpty(_baseCurrencyCode)? Order.Currency.ISOCode : _baseCurrencyCode; }
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
            TransactionAffiliation = "Big Bus Tours Ticket Store";
            if (Order.Total != null) TransactionTotal = Order.Total.Value;
            TransactionTax = 0;
            TransactionShipping = 0;
            TransactionCurrency = Order.Currency.ISOCode ;

            ecommerceTracking.Visible = true;
            _baseCurrencyCode = GetLastOrderLineCurrencyCode();

        }

        public string GetLastOrderLineCurrencyCode()
        {
            try
            {
                var orderLines = Order.OrderLines;
                var topLine = orderLines.OrderBy(x => x.ExternalOrder).FirstOrDefault();

                if (topLine == null) return null;

                var microsite = BasePage.SiteService.GetMicroSiteById(topLine.MicrositeId);
                return microsite != null ? BasePage.CurrencyService.GetCurrencyIsoCodeById(microsite.CurrencyId) : null;
            }
            catch (Exception ex)
            {
                BasePage.Log("TagManager => GetLastOrderLineCurrencyCode() failed orderId: " + Order.Id + " ex " + ex.Message);
                return string.Empty;
            }
        }

        public string MakeOrderLines()
        {
            var sbTemp = new StringBuilder();
            var orderLines = Order.OrderLines;

            foreach (var orderline in orderLines)
            {
                var ticket = BasePage.TicketService.GetTicketById(orderline.TicketId.ToString());
               
                if (sbTemp.Length > 10)
                {
                    sbTemp.AppendLine(",");
                }

                sbTemp.AppendLine("{'name': '" + ticket.Name.Replace("'", "\'") + "',");
                sbTemp.AppendLine("'id': '" + ticket.Id +"',");
                sbTemp.AppendLine("'price': '" + ConvertPriceToBaseCurrency(orderline.TicketCost.Value) + "',");
                sbTemp.AppendLine("'brand': 'Big Bus Tours',");
                sbTemp.AppendLine("'category': '" + orderline.TicketTorA + "',");
                sbTemp.AppendLine("'variant': '" + orderline.TicketType + "',");
                sbTemp.AppendLine("'quantity': " + orderline.TicketQuantity + ",");
                sbTemp.AppendLine("'coupon': '" + orderline.ExternalCoupon + "'");//use coupon from BORN
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
                    'currencyCode': '" + BaseCurrencyCode + "'," +                                                    
                        @"'purchase': {
                            'actionField': {
                                'id': '" + TransactionId + "'," + Environment.NewLine +
                                 "'affiliation': '" + TransactionAffiliation + "'," + Environment.NewLine +
                                 "'revenue': '" + ConvertPriceToBaseCurrency(TransactionTotal) + "'," + Environment.NewLine +
                                 "'tax':'" + ConvertPriceToBaseCurrency(TransactionTax) + "'," + Environment.NewLine +
                                 "'shipping': '" + ConvertPriceToBaseCurrency(TransactionShipping) + "'," + Environment.NewLine +
                                 "'coupon': '" + Order.ExternalCoupon + "'" + Environment.NewLine +
                             @"},                    
                            'products': [" + MakeOrderLines() + @"]
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
                if (TransactionCurrency.Equals(BaseCurrencyCode, StringComparison.CurrentCultureIgnoreCase))
                    return price;

                var baseCurrencyConfigValue = Convert.ToDecimal(ConfigurationManager.AppSettings["GA." + BaseCurrencyCode]);
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