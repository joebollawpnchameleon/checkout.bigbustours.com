using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.Controllers;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Enums;
using Services.Infrastructure;
using Services.Implementation;
using System.Text;
using Common.Model;
using bigbus.checkout.EcrWServiceRefV3;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace bigbus.checkout
{
    public partial class Default : BasePage
    {
        protected int ItemIndex = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            dvError.Visible = false;

            if (!IsPostBack)
            {
                if (ConfigurationManager.AppSettings["Testing"] == null || !ConfigurationManager.AppSettings["Testing"].Equals("1"))
                {
                    Response.Redirect("~/bookingaddress.aspx");
                    return;
                }

                LoadTicketList();

                //txtCookieValue.Text = "0%253A2%253AzdqzxUdeGzV1wCalCSxHJRizo8AtSVKi%253ABwmx0AmImyc%252FLkkGRnfhpdhbq%252B3JpOCVhdWXyHlwS9U%253D"; // GetLatestIncreasedCookieValue();
                PlantCookie(sender, e);
                //LoadBasketLines();
            }
        }

        protected void ChangeCurrency(object sender, EventArgs e)
        {
            LoadTicketList();
        }

        protected void LoadTicketList()
        {
            var productList = EcrService.GetProductList(CacheProvider);

            var ticketList = TicketService.GetTestTickets();

            foreach(var ticketDetails in ticketList)
            {
                var product = productList.FirstOrDefault(x => x.SysID.Equals(ticketDetails.EcrProductCode, StringComparison.CurrentCultureIgnoreCase));
                ParseCsvIntoOptionList(ticketDetails, product);
            }

            rptItems.DataSource = ticketList; // MagentoTestController.TestBasketItems;
            rptItems.DataBind();
        }

        protected void ParseCsvIntoOptionList(TestTicket ticketDetails, Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(ticketDetails.AdditionalDetailsCsv) || !ticketDetails.AdditionalDetailsCsv.Contains(","))
                    return;

                var sbTemp = new StringBuilder();
                var sbQuantHtml = new StringBuilder();
                var sbTotalHtml = new StringBuilder();

                var allOptions = ticketDetails.AdditionalDetailsCsv.Split(',');

                foreach (var option in allOptions)
                {
                    if (string.IsNullOrEmpty(option))
                        continue;
                    var arr = option.Split('|');
                    var childSku = arr[0];

                    var productDimension = product.ProductDimensions.FirstOrDefault(x => x.SysID.Equals(childSku, StringComparison.CurrentCultureIgnoreCase));

                    if (productDimension == null || productDimension.Prices == null || !productDimension.Prices.Any())
                        continue;

                    var currency = CurrencyService.GetCurrencyByCode(ddlCurrency.SelectedValue);

                    var price = productDimension.Prices.FirstOrDefault(x => x.CurrencyCode.Equals(currency.ISOCode, StringComparison.CurrentCultureIgnoreCase));

                    sbTemp.AppendLine(string.Format("{0} <b> {1}{2}</b> <br/>", arr[1], currency.Symbol, price.Amount));

                    sbQuantHtml.AppendLine(string.Format(@"<b class=""minus"" onclick=""ChangeValue('{0}', -1, '{1}')"" >-</b> &nbsp;<b class=""plus"" onclick=""ChangeValue('{0}', 1, '{1}')"" >+ <span id=""sp_quantity_{0}""></span></b><br/>",
                        childSku, product.SysID));

                    sbTotalHtml.AppendLine(string.Format(@"<input type=""hidden"" id=""hdn_price_{0}"" value=""{1}""/><input type=""hidden"" id=""hdn_currency_{0}"" value=""{2}"" /><span id=""sp_total_{0}""></span><br/>", 
                        childSku, price.Amount, currency.Symbol));
                }

                ticketDetails.PriceOptionHtml = sbTemp.ToString();
                ticketDetails.QuantityOptionHtml = sbQuantHtml.ToString();
                ticketDetails.TotalOptionHtml = sbTotalHtml.ToString();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        protected string GetLatestIncreasedCookieValue()
        {
            var basket = BasketService.GetLatestBasket();
            return basket != null ? IncreaseCookieValue(basket.ExternalCookieValue) : "001";
        }

        protected void PlantCookie(object sender, EventArgs e)
        {
            txtCookieValue.Text = GetLatestIncreasedCookieValue();
            //txtCookieValue.Text = "0%253A2%253AzdqzxUdeGzV1wCalCSxHJRizo8AtSVKi%253ABwmx0AmImyc%252FLkkGRnfhpdhbq%252B3JpOCVhdWXyHlwS9U%253D";
            AuthenticationService.SetCookie(ExternalBasketCookieName, SessionCookieDomain, txtCookieValue.Text);
        }

        private string IncreaseCookieValue(string cookieValue)
        {
            if (string.IsNullOrEmpty(cookieValue) || cookieValue.Length < 3)
                return string.Empty;

            var val = cookieValue.Substring(2);
            var newval = Convert.ToInt32(val);
            newval++;
            return cookieValue.Replace(val, newval.ToString());
        }

        protected void GoToCheckout(object sender, EventArgs e)
        {
            Response.Redirect("~/BookingAddress.aspx");
        }

        protected void PostUserToDetailsPage(object sender, EventArgs e)
        {
            var jsonSelection = hdnSelections.Value;

            if(string.IsNullOrEmpty(jsonSelection))
            {
                ltError.Text = "Please select some products before proceeding";
                dvError.Visible = true;
                return;
            }

            var allJson = new StringBuilder();
            decimal total = 0.0m;
            var productList = EcrService.GetProductList(CacheProvider);
            const string itemstr = @"""name"":""{0}"",""sku"":""{1}"",""ProductDimensionUID"":""{2}"",""qty"":{3},""price"":{4},""discount"":0.00,""total"":{5},""city"":""{6}"",""type"":""{7}"", ""MainProductSKU"":""{8}""";
            var lastMicrosite = string.Empty;

            dynamic dynJson = JsonConvert.DeserializeObject(jsonSelection);
            dynamic selection = dynJson.selection;

            foreach(var item in selection)
            {
                var productsku = item.productsku.ToString();
                var selections = item.selections;
                var product = productList.FirstOrDefault(x => x.SysID.Equals(productsku, StringComparison.CurrentCultureIgnoreCase));
                
                foreach (var sel in selections)
                {
                    var price = Convert.ToDecimal(sel.price);
                    var quantity = Convert.ToInt32(sel.quantity);
                    var localsku = sel.sku.ToString();
                    var city = sel.city.ToString();
                    lastMicrosite = city;
                    total += (price * quantity);

                    var productDimension = product.ProductDimensions.FirstOrDefault(x => x.SysID.Equals(localsku, StringComparison.CurrentCultureIgnoreCase));
                    var priceDimension = productDimension.Prices.FirstOrDefault(x => x.CurrencyCode.Equals(ddlCurrency.SelectedValue));
                    var temp = "{" + string.Format(itemstr, product.Name, localsku, priceDimension.ProductDimensionUID, quantity.ToString(), price.ToString(), (price * quantity).ToString(), city, productDimension.Name, productsku) + "}";
                    if (allJson.Length > 10)
                        allJson.Append(",");

                    allJson.AppendLine(temp);
                }               
            }

            allJson.Insert(0, @"{
                            ""items"":[").AppendLine("]," + System.Environment.NewLine +
                            string.Format(@"""subtotal"":{0},""discount"":{1},""total"":{2},""coupon"":""{3}"",""currency"":""{4}"",""language"":""{5}"",""store"":""{6}""",
                            total, "0.0", total, "null", ddlCurrency.SelectedValue, ddlLanguage.SelectedValue, lastMicrosite) + System.Environment.NewLine + "}");

            LoggerService.LogBornBasket(allJson.ToString(), ExternalSessionCookieValue);
        
            Response.Redirect("~/BookingAddress.aspx");
        }
        
    }

    public class TestBasket
    {
        //[JsonProperty("currency")]
        //public string CurrencyCode { get; set; }

        //[JsonProperty("language")]
        //public string Language { get; set; }

        [JsonProperty("selection")]
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        [JsonProperty("productsku")]
        public string ProductSku { get; set; }  

        [JsonProperty("selections")]
        public List<LineItem> LineItems { get; set; }

        //#region JsonIgnoredProperties

        //[JsonIgnore]
        //public Guid CurrencyId { get; set; }

        //[JsonIgnore]
        //public bool IsValid { get; set; }

        //[JsonIgnore]
        //public string ExternalCookieValue { get; set; }

        //#endregion

    }

    public class LineItem
    {      
       
        [JsonProperty("sku")]
        public string Sku { get; set; }
        
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public decimal UnitCost { get; set; }

        //[JsonProperty("discount")]
        //public decimal Discount { get; set; }

        //[JsonProperty("coupon")]
        //public string Coupon { get; set; }

        //[JsonProperty("total")]
        //public decimal Total { get; set; }

        //[JsonProperty("city")]
        //public string Microsite { get; set; }
        
        //[JsonProperty("type")]
        //public TicketVariation TicketType { get; set; }

    }

}