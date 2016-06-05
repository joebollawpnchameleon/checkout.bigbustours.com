using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace bigbus.checkout.Controls
{
    public partial class AdvertiserTracking : BaseControl
    {
        public void Page_PreRender(object o, EventArgs a)
        {
           
            ecommerceTracking.Visible = false;

            if (!string.IsNullOrWhiteSpace(OrderId) && BasePage.ShowAffiliateWindow)
            {
                if (Order == null)
                {
                    Order =  BasePage.CheckoutService.GetFullOrder(OrderId);
                }

                if (Order != null)
                {
                    // we are only rendering when the source is awin
                    var awin = SettingsHelper.GlobalSetting("AffiliateWindowSource", "awin");

                    var source = _affiliates.GetAffilliateNetworkSource();

                    if (source != null && source.Equals(awin, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var orderLines = Order.OrderLines;

                        var parts = new Dictionary<string, decimal>();

                        foreach (var line in orderLines)
                        {
                            var orderLine = line as OrderLine;

                            if (orderLine != null)
                            {
                                var key = GetAttractionsOrTour(orderLine);

                                if (!parts.ContainsKey(key))
                                {
                                    parts.Add(key, 0);
                                }

                                parts[key] += (decimal)(orderLine.TicketCost.Value * orderLine.TicketQuantity);
                            }
                        }

                        var partsSb = new StringBuilder();

                        foreach (var key in parts.Keys)
                        {
                            if (partsSb.Length > 0)
                            {
                                partsSb.Append("|");
                            }

                            partsSb.AppendFormat("{0}:{1}", key, parts[key]);
                        }

                        PartsString = partsSb.ToString();

                        // Start -> _addTrans
                        OrderRef = Order.OrderNumber.ToString(CultureInfo.InvariantCulture);

                        OrderSubtotal = Order.Total.Value;
                        CurrencyCode = Order.Currency.ISOCode;
                        SaleAmount = Order.Total.Value;
                        VoucherCode = string.Empty;

                        if (orderLines.Any(x => !string.IsNullOrWhiteSpace(x.PromotionId)))
                        {
                            VoucherCode = orderLines.First(x => !string.IsNullOrWhiteSpace(x.PromotionId)).PromotionId;
                        }

                        basketRows.DataSource = orderLines;
                        basketRows.DataBind();

                        ecommerceTracking.Visible = true;
                    }
                }
            }
        }

        public string GetAttractionsOrTour(OrderLine toa)
        {
            if (toa.TicketTorA.Equals("ATTRACTION", StringComparison.InvariantCultureIgnoreCase))
            {
                return BasePage.CurrentSite.AffiliateWindowAttractionCommissionLabel;
            }
            else
            {
                return BasePage.CurrentSite.AffiliateWindowTourCommissionLabel;
            }
        }

        public Order Order { get; set; }

        /// <summary>
        /// Set this way on the confirmation page only
        /// </summary>
        public string OrderId { get; set; }

        public string MerchantId
        {
            get
            {
                return BasePage.CurrentSite.AffiliateWindowMerchantId;
            }
        }

        public string IsTest
        {
            get { return SettingsHelper.GlobalSetting("AffiliateWindow-IsTest", "0"); }
        }

        public string Channel
        {
            get { return SettingsHelper.GlobalSetting("AffiliateWindow-Channnel", "aw"); }
        }

        private readonly Affiliates _affiliates = new Affiliates();

        public string PartsString { get; set; }
        public decimal OrderSubtotal { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderRef { get; set; }
        public decimal SaleAmount { get; set; }
        public string VoucherCode { get; set; }
    }
}