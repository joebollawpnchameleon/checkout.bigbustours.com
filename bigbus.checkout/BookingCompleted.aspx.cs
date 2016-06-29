using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.Controls;
using bigbus.checkout.Controls.Google;
using BigBusWebsite.controls;
using Clockwork;
using Services.Implementation;

namespace bigbus.checkout
{
    public partial class BookingCompleted : BasePage
    {
        private string _useremail;
        private Order _order;
        private SiteMaster _bookingMaster;
        

        protected string IntileryTagScript { get; set; }
        protected string UserEmail
        {
            get { return _useremail; }
        }

        protected string DownloadFreeAppText
        {
            get
            {
                var text = GetTranslation("ViewBusStopsDownload");
                var appleLink = "<a href=\"" + MakeAppleDownloadUrl() + "\">App Store</a>";
                var googlePlayLink = "<a href=\"" + MakeGooglePlayDownloadUrl() + "\">Google Play</a>";

                return !string.IsNullOrEmpty(text) ?
                    string.Format(text, appleLink, googlePlayLink, MicrositeId) : text;
            }    
        }

        public bool ShowMobile { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            _bookingMaster = (SiteMaster)Master;
           
            var orderId = string.IsNullOrEmpty(Request.QueryString["oid"])? hdnOrderId.Value : Request.QueryString["oid"];

            if (string.IsNullOrEmpty(orderId)) 
                return;

            _order = CheckoutService.GetFullOrder(orderId);

            if (_order == null)
            {
                //lbError.Text = "Invalid order request.";//*** translation needed
                //lbError.Visible = true;
                Log("Invalid order request orderid: " + orderId);
                return;
            }
            PreparePage(_order);
            TrackOrderCompleted(_order);
        }
        

        private void TrackOrderCompleted(Order order)
        {
            try
            {
                var orderLines = order.OrderLines;
                var temp = orderLines.Select(x => x.MicrositeId).ToList();
                var shippingCities = string.Join(",", temp.ToArray());
                var allScripts = new StringBuilder();


                var transactionOrder = new IntileryTagHelper.TransactionOrder
                {
                    OrderNumber = order.OrderNumber.ToString(),
                    OrderTax = (decimal)0.0,
                    OrderTotal = order.Total.Value,
                    ShippingCity = shippingCities,
                    StoreName = "BigBusTours",
                    TransactionCurrency = order.Currency.ISOCode
                };

                if (order.User != null)
                {
                    hdnUserEmail.Value = order.User.Email;

                    var language = TranslationService.GetLanguage(order.LanguageId);

                    transactionOrder.Customer = new IntileryTagHelper.TransactionCustomer
                    {
                        FirstName = order.User.Firstname,
                        LastName = order.User.Lastname,
                        Email = order.User.FriendlyEmail,
                        Language = language.ShortCode,
                        Address1 = order.User.AddressLine1,
                        Address2 = order.User.AddressLine2,
                        Town = order.User.City,
                        PostCode = order.User.PostCode,
                        Country = order.User.CountryId
                    };
                }

                allScripts.AppendLine(IntileryTagHelper.TrackAddTrans(transactionOrder));

                //add orderlines
                foreach (var line in orderLines)
                {
                    var transactionItem = new IntileryTagHelper.TransactionOrderItem
                    {
                        OrderNumber = order.OrderNumber.ToString(),
                        ItemCategory = line.TicketTorA,
                        ItemName = line.Ticket.Name,
                        ItemPrice = line.TicketCost.Value,
                        ItemQuantity = line.TicketQuantity.Value,
                        ItemSku = line.TicketId.ToString(),
                        ProductItem = new IntileryTagHelper.ProductBasketLine
                        {
                            ProductId = line.TicketId.ToString(),
                            LineTotal = line.NettOrderLineValue.ToString(),
                            PassengerType = line.TicketType,
                            ProductLanguage = CurrentLanguageId,
                            ProductName = line.Ticket.Name,
                            ProductSupplierCode = line.TicketId.ToString(),
                            Quantity = line.TicketQuantity.Value,
                            TicketType = line.TicketTorA
                        }
                    };

                    allScripts.AppendLine(IntileryTagHelper.TrackAddTransItem(transactionItem));
                }

                IntileryTagScript = allScripts.ToString();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            if (chkReceiveNews.Checked)
            {
                User user = null;

                try
                {
                    user = UserService.GetUserByEmail(hdnUserEmail.Value);
                }
                catch
                {
                    //ignore
                }

                if (user != null)
                {
                    user.ReceiveNewsletter = true;
                    UserService.SaveUser(user);
                }
            }

            if (hdnIsMobileAppSession.Value.Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
            {
                Response.Redirect("bbt://open-tour/" + MicrositeId);
            }
            else
            {
                RedirectToHomePage();
            }
        }

        private void PreparePage(Order order)
        {
            var session = GetSession();

            ltlOrderNumber.Text = order.OrderNumber.ToString();

            //***MobileAppSessionMadness(session);

            DisplayMarketingScript();

            CheckIfMobileSectionShouldBeShown(order);
            
            //by getting the details for the order from the database
            eVoucherLink.NavigateUrl = ResolveUrl("~/viewvoucher.aspx?oid=" + order.Id + "&name=file.pdf&browser=true");

            _useremail = order.EmailAddress;

            UpdateAnalyticsTrackers(order);
        }

        private void CheckIfMobileSectionShouldBeShown(Order order)
        {
            var showMobile = CheckoutService.OrderAllTicketShowMobile(order);

            ShowMobile = showMobile && CurrentSite.UseQR;

            plhShowMobile.Visible = ShowMobile;

            if (!ShowMobile) return;

            if (!IsPostBack)
            {
                var dialcodes = CheckoutService.GetAlldiallingDiallingCodes();

                var allDialingCodeList = dialcodes;

                var usa = allDialingCodeList.FirstOrDefault(x => x.Id == "United States of America");
                var uk = allDialingCodeList.FirstOrDefault(x => x.Id == "United Kingdom");
                var australia = allDialingCodeList.FirstOrDefault(x => x.Id == "Australia");
                var canada = allDialingCodeList.FirstOrDefault(x => x.Id == "Canada");
                var france = allDialingCodeList.FirstOrDefault(x => x.Id == "France");
                var germany = allDialingCodeList.FirstOrDefault(x => x.Id == "Germany");
                var hongKong = allDialingCodeList.FirstOrDefault(x => x.Id == "Hong Kong");
                var unitedArabEmirates = allDialingCodeList.FirstOrDefault(x => x.Id == "United Arab Emirates");

                var stupidSelectSeperatorWithNothingItem = new DiallingCode
                {
                    Id = "=================",
                    Code = int.MinValue
                };

                var newDiallingCodeList = new List<DiallingCode>
                {
                    usa,
                    uk,
                    australia,
                    canada,
                    france,
                    germany,
                    hongKong,
                    unitedArabEmirates,
                    stupidSelectSeperatorWithNothingItem
                };

                newDiallingCodeList.AddRange(allDialingCodeList);

                var defaultcode = CurrentSite.IsUS ? "United States of America" : "United Kingdom";

                foreach (var diallingCode in newDiallingCodeList)
                {
                    var item = new ListItem(diallingCode.Id, diallingCode.Id);

                    if (diallingCode.Code == int.MinValue)
                    {
                        item.Attributes["disabled"] = "disabled";
                    }

                    DiallingList.Items.Add(item);
                }

                DiallingList.SelectedValue = defaultcode;
            }

            var dialCode = CheckoutService.GetDiallingCode(DiallingList.SelectedItem.Value);
            CountryCode.Value = "+" + dialCode.Code;
        }

        protected void SendToMobileClick(object sender, EventArgs e)
        {
            if (ShowMobile)
            {
                LIMobileError.Text = string.Empty;

                if (string.IsNullOrWhiteSpace(Mobile.Value))
                {
                    LIMobileError.Text = "<p style=\"color:red; margin-top:0!important\">" + GetTranslation("Booking_MobileNumberError") + "</p>";
                }
                else
                {
                    var pointlessLong = long.MinValue;

                    if (long.TryParse(Mobile.Value, out pointlessLong))
                    {
                        //UseOrderQrCode(_order);

                        var qrCodeHash = Crypto.HashPassword(_order.CentinelAcsurl + _order.OrderNumber);

                        _order.SentQrCodeToMobile = true;
                        _order.QrCodeUniqueHash = qrCodeHash;
                        CheckoutService.SaveOrder(_order);

                        var qrCodeUrl = string.Format("QRCode.aspx?oid={0}&id={1}", _order.Id, Uri.EscapeDataString(qrCodeHash));
                        
                        if (qrCodeUrl.StartsWith("/"))
                        {
                            //var currentPageUrl = Response.Headers["HTTP_X_ORIGINAL_URL"];

                            //var hostPartOfUrl = currentPageUrl.Substring(0, currentPageUrl.IndexOf("/" + MicrositeId, StringComparison.Ordinal));

                            qrCodeUrl = BaseUrl + qrCodeUrl;
                        }

                        string tinyUrl;

                        using (var client = new WebClient())
                        {
                            var postData = new NameValueCollection() { { "url", qrCodeUrl } };

                            // client.UploadValues returns page source as byte array (byte[])
                            // so we need to transform that into string
                            tinyUrl =
                                Encoding.UTF8.GetString(client.UploadValues("http://tinyurl.com/api-create.php", postData));
                        }

                        var dialCode = CheckoutService.GetDiallingCode(DiallingList.SelectedItem.Value);
                        var mobilenum = dialCode.Code.ToString();

                        if (mobilenum.Trim().Equals("44") && Mobile.Value.StartsWith("0"))
                        {
                            mobilenum += Mobile.Value.Trim().Substring(1);
                        }
                        else
                        {
                            mobilenum += Mobile.Value.Trim();
                        }

                        var mobiletext =
                            "Thank you for booking with Big Bus Tours" + Environment.NewLine +
                            "Order number: {0}" + Environment.NewLine +
                            "{1} tickets in total" + Environment.NewLine +
                            "Click the link below to view your ticket" + Environment.NewLine +
                            "{2}";

                        mobiletext = string.Format(mobiletext, _order.OrderNumber, _order.TotalQuantity, tinyUrl);

                        var api = new API(ConfigurationManager.AppSettings["Mobile_SMS_APIKey"]);

                        try
                        {
                            var result = api.Send(new SMS { To = mobilenum, Message = mobiletext, From = "BigBusTours" });

                            if (result.Success)
                            {
                                LIMobileError.Text = "<p style=\"color:green; margin-top:0!important\">" +
                                                     GetTranslation("Booking_MobileSuccess") + "</p>";
                            }
                            else
                            {
                                LIMobileError.Text = "<p style=\"color:red; margin-top:0!important\">" +
                                                     GetTranslation("AnErrorOccuredPleaseTryAgainLater") + "</p>";
                                Log(string.Format("== Clockwork error. Error code: {0}, Error message: {1}",
                                                  result.ErrorCode,
                                                  result.ErrorMessage));
                            }
                        }
                        catch (WebException ex)
                        {
                            LIMobileError.Text = "<p style=\"color:red; margin-top:0!important\">" +
                                                 GetTranslation("AnErrorOccuredPleaseTryAgainLater") + "</p>";
                            // Web exceptions mean you couldn't reach the Clockwork server
                            Log(
                                string.Format(
                                    "== Web exceptions mean you couldn't reach the Clockwork server. Exception: {0}",
                                    ex.Message));
                        }
                        catch (Exception ex)
                        {
                            LIMobileError.Text = "<p style=\"color:red; margin-top:0!important\">" +
                                                 GetTranslation("Booking_MobileNumberError") + "</p>";
                            // Something else went wrong, the error message should help here
                            Log(
                                string.Format(
                                    "== Something else went wrong, the error message should help here. Exception: {0}",
                                    ex.Message));
                        }
                    }
                    else
                    {
                        LIMobileError.Text = "<p style=\"color:red; margin-top:0!important\">" + GetTranslation("Booking_MobileNumberError") + "</p>";
                    }
                }
            }
        }

        private void DisplayMarketingScript()
        {
            string comment = null;
            string conversionlabel = null;
            string conversionid = null;

            var subsite = MicrositeId.ToLower();

            switch (subsite)
            {
                case "london":
                    {
                        //London successful Checkout script
                        DisplaySurveyLink.Visible = true;

                        comment = "<!--- London successful Checkout ---><!-- Google Code for London Negative List Remarketing List -->";
                        conversionlabel = "Ri7WCMyskwIQ-quV8AM";
                        conversionid = "1040537082";
                        break;
                    }

                case "dubai":
                    {
                        //dubai successful Checkout script
                        comment = @"<!--- Dubai successful Checkout ---><!-- Google Code for Dubai Negative List Remarketing List -->";
                        conversionlabel = @"WYpyCMStkwIQ-quV8AM";
                        conversionid = @"1040537082";
                        break;
                    }

                case "lasvegas":
                    {
                        DisplaySurveyLink.Visible = true;
                        lnkSurveyMonkey.NavigateUrl = "https://www.surveymonkey.com/s/las-vegas-post";
                        break;
                    }

                case "newyork":
                    {
                        DisplaySurveyLink.Visible = true;
                        lnkSurveyMonkey.NavigateUrl = "https://www.surveymonkey.com/s/new-york-post";
                        break;
                    }

                case "sanfrancisco":
                    {
                        DisplaySurveyLink.Visible = true;
                        lnkSurveyMonkey.NavigateUrl = "https://www.surveymonkey.com/s/san-fran-post";
                        break;
                    }

                case "miami":
                    {
                        DisplaySurveyLink.Visible = true;
                        lnkSurveyMonkey.NavigateUrl = "https://www.surveymonkey.com/s/miami-post";
                        break;
                    }

                case "washington":
                    {
                        DisplaySurveyLink.Visible = true;
                        lnkSurveyMonkey.NavigateUrl = "https://www.surveymonkey.com/s/dc-post";
                        break;
                    }

                default:
                    {
                        return;
                    }
            }

            if (conversionid == null)
            {
                return;
            }

            marketingscripts.Text = GetMarketingScript(comment, conversionid, conversionlabel);
        }

        private static string GetMarketingScript(string comment, string conversionid, string conversionlabel)
        {
            var marketingScript =
                comment +
                @"<script type='text/javascript'>
                    /* <![CDATA[ */
                    var google_conversion_id = " + conversionid + @";
                    var google_conversion_language = 'en';
                    var google_conversion_format = '3';
                    var google_conversion_color = '666666';
                    var google_conversion_label = '" + conversionlabel + @"';
                    var google_conversion_value = 0;
                    /* ]]> */
                </script>
                <script type='text/javascript' src='https://www.googleadservices.com/pagead/conversion.js'></script>
                <noscript>
                    <div style='display:inline;'>
                        <img height='1' width='1' style='border-style:none;' alt='' src='https://www.googleadservices.com/pagead/conversion/" +
                            conversionid +
                            @"/?label=" +
                            conversionlabel +
                            @"&amp;guid=ON&amp;script=0'/>
                    </div>
                </noscript>
                <!--- End --->
                ";

            return marketingScript;
        }

        private void UpdateAnalyticsTrackers(Order order)
        {
            if (Master == null || order.OrderConfirmationViewed)
                return;
            
            //do tag manager tracking
            try
            {
                var tagMangerControl = Master.FindControl("gglTagManager") as TagManager;

                if (tagMangerControl == null)
                {
                    Log(
                        "BookingCompleted => UpdateAnalyticsTrackers() - TagManager control 'gglTagManager' missing. OrderId = " +
                        order.Id);
                }
                else
                    tagMangerControl.OrderId = order.Id.ToString();
            }
            catch (Exception ex)
            {
                Log("UpdateAnalyticsTrackers() - " +  ex.Message);
            }

            // affiliatewindow trackers
            try
            {
                var adTracker = Master.FindControl("afAdvertiserTracking") as AdvertiserTracking;
                    
                if (adTracker == null)
                {
                    Log(
                        "BookingCompleted => UpdateAnalyticsTrackers() - AdvertiserTracking control 'afAdvertiserTracking' missing. OrderId = " +
                        order.Id);
                }
                else
                    adTracker.OrderId = order.Id.ToString();
            }
            catch (Exception ex)
            {
                    Log("UpdateAnalyticsTrackers() - " +  ex.Message);
            }
                  
            try
            {
                var eptecaImageControl = Master.FindControl("EptecaImage1") as EptecaImage;

                if (eptecaImageControl == null)
                {
                    Log(
                       "BookingCompleted => UpdateAnalyticsTrackers() - EptecaImage control 'EptecaImage1' missing. OrderId = " +
                       order.Id);
                }else
                    eptecaImageControl.OrderId = order.Id.ToString();
            }
            catch(Exception ex)
            {
                Log("UpdateAnalyticsTrackers() - " + ex.Message);
            }

            try
            {
                CommissionJunctionTag.Order = order;
            }
            catch (Exception ex)
            {
                Log("UpdateAnalyticsTrackers() - failed for CommissionJunctionTag" + ex.Message);
            }

            order.OrderConfirmationViewed = true;
            CheckoutService.SaveOrder(order);
        }

    }
}