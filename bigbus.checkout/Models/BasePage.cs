
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Services.Infrastructure;
using Autofac.Integration.Web;
using System.Web;
using Autofac;
using bigbus.checkout.Controls;
using bigbus.checkout.Helpers;
using bigbus.checkout.data.Model;
using Common.Enums;
using bigbus.checkout.EcrWServiceRefV3;
using Common.Helpers;
using Common.Model;
using System.Data;
using Common.Model.Interfaces;
using BigBusWebsite.controls.SharedLayout;
using System.Collections.Generic;
using bigbus.checkout.data.PlainQueries;

namespace bigbus.checkout.Models
{
    public class BasePage : System.Web.UI.Page
    {        
        #region Injectable properties (need to be public)

        public IApiConnectorService ApiConnector { get; set; }
        public IBasketService BasketService { get; set; }
        public ICountryService CountryService { get; set; }
        public IUserService UserService { get; set; }
        public IPciApiServiceNoASync PciApiService { get; set; }
        public ICurrencyService CurrencyService { get; set; }
        public ITicketService TicketService { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }
        public ISiteService SiteService { get; set; }
        public ILoggerService LoggerService { get; set; }
        public ITranslationService TranslationService { get; set; }
        public IPaypalService PaypalService { get; set; }
        public IImageDbService ImageDbService { get; set; }
        public IImageService ImageService { get; set; }
        public IEcrService EcrService { get; set; }
        public ICheckoutService CheckoutService { get; set; }
        public INotificationService NotificationService { get; set; }
        public ILocalizationService LocalizationService { get; set; }
        public IBarcodeService BarcodeService { get; set; }
        public ICacheProvider CacheProvider { get; set; }
        public INavigationService NavigationService { get; set; }

        #endregion

        private MicroSite _currentSite;

        public string ExternalBasketCookieName { get { return ConfigurationManager.AppSettings["External.Basket.CookieName"]; } }
        public string SessionCookieName { get { return ConfigurationManager.AppSettings["Session.CookieName"]; } }
        public string SessionCookieDomain { get { return ConfigurationManager.AppSettings["Session.CookieDomain"]; } }
        public string PciApiDomain{ get { return ConfigurationManager.AppSettings["PciWebsite.ApiDomain"]; } }
        public string PciDomain { get { return ConfigurationManager.AppSettings["PciWebsite.Domain"]; } }
        public string PciLandingPagePath { get { return ConfigurationManager.AppSettings["PciWebsite.LandingPagePath"]; } }
        public string BasketCookieName { get { return ConfigurationManager.AppSettings["Basket.CookieName"]; } }
        public string GoogleChartUrl { get { return ConfigurationManager.AppSettings["GoogleChartUrl"]; } }
        public string LiveEcrEndPoint
        {
            get { return ConfigurationManager.AppSettings["LiveEcrEndPoint"]; }
        }
        public string EcrApiKey { get { return ConfigurationManager.AppSettings["EcrApiKey"]; } }
        public int EnvironmentId { get { return (ConfigurationManager.AppSettings["Environment"] != null)? Convert.ToInt32(ConfigurationManager.AppSettings["Environment"]) : (int)Common.Enums.Environment.Local; } }

        public string CurrentLanguageId { get { return "eng"; } } //***replace with function
        public string MicrositeId { get { return "london"; } } //*** get from url as in function
        public string SubSite { get { return "london"; } }  //*** work out from old code

        public bool SiteIsUs(ISiteService siteService)
        {
            return siteService.GetMicroSiteById(MicrositeId).IsUS;
        }

        public MicroSite CurrentSite
        {
            get
            {
                if(_currentSite == null)
                    _currentSite = SiteService.GetMicroSiteById(MicrositeId);

                return _currentSite;
            }
        }

        public bool ShowAffiliateWindow
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(CurrentSite.AffiliateWindowMerchantId) &&
                    !string.IsNullOrWhiteSpace(CurrentSite.AffiliateWindowAttractionCommissionLabel) &&
                    !string.IsNullOrWhiteSpace(CurrentSite.AffiliateWindowTourCommissionLabel);
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
            var cp = cpa.ContainerProvider;
            cp.RequestLifetime.InjectProperties(this);
            LoadMasterValues();
        }

        public string GetTranslation(string keyPhrase)
        {
            return TranslationService.TranslateTerm(keyPhrase, CurrentLanguageId);
        }

        public void Log(string message)
        {
            LoggerService.LogItem(message);
        }

        public string GetClientIpAddress()
        {
            var clientIpAddress = Request.Headers["X-Forwarded-IP"];

            if (string.IsNullOrEmpty(clientIpAddress))
            {
                clientIpAddress = Request.ServerVariables["X-Forwarded-IP"];
            }

            if (string.IsNullOrEmpty(clientIpAddress))
            {
                clientIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_IP"];
            }

            if (string.IsNullOrEmpty(clientIpAddress))
            {
                clientIpAddress = Request.Headers["X-Forwarded-For"];
            }

            if (string.IsNullOrEmpty(clientIpAddress))
            {
                clientIpAddress = Request.ServerVariables["X-Forwarded-For"];
            }

            if (string.IsNullOrEmpty(clientIpAddress))
            {
                clientIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            if (string.IsNullOrEmpty(clientIpAddress))
            {
                clientIpAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            if (clientIpAddress.Contains(","))
            {
                clientIpAddress = clientIpAddress.Split(',').FirstOrDefault();
            }
            return clientIpAddress;
        }

        public void GenerateOrderBarcodes(Order order)
        {

            try
            {
                var orderLines = order.OrderLines;

                if (orderLines != null && orderLines.Count > 0)
                {
                    foreach (var orderline in orderLines)
                    {
                        var orderLineGenerateBarCodes = CheckoutService.GetOrderLineGeneratedBarcodes(orderline);

                        for (var y = 0; y < orderline.TicketQuantity; y++)
                        {
                            if (orderLineGenerateBarCodes == null || orderLineGenerateBarCodes.Count == 0)
                            {
                                var barcode = BarcodeService.GetNextBarcode(orderline.Ticket, orderline.TicketType).Substring(0, 12);

                                var orderlineGeneratedBarcode = new OrderLineGeneratedBarcode();

                                orderlineGeneratedBarcode.OrderLineId = orderline.Id;

                                orderlineGeneratedBarcode.GeneratedBarcode =
                                    barcode +
                                    CalculateBarcodeChecksum(barcode.Substring(0, 12));

                                CheckoutService.SaveOrderLineBarCode(orderlineGeneratedBarcode);
                            }
                        }
                    }
                }
            }
            catch
            {
                Log("***Exception : GenerateOrderBarcodes for the current orderid:" + order.Id);
            }
        }
       

        /// <summary>
        /// This function calculates the checksum digit for a barcode
        /// </summary>
        public static int CalculateBarcodeChecksum(string code)
        {
            if (code == null || code.Length != 12)
                throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit");

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int v;
                if (!int.TryParse(code[i].ToString(), out v))
                    throw new ArgumentException("Invalid character encountered in specified code.");
                sum += (i % 2 == 0 ? v : v * 3);
            }
            int check = 10 - (sum % 10);
            return check % 10;
        }


        protected EcrResult SendBookingToEcr(Order order)
        {
            Log("Sending booking to ECR");

            var orderId = order.Id.ToString();
            var orderLineDetails = CheckoutService.GetOrderLineDetails(orderId);

            if (orderLineDetails == null)
            {
                Log("Could not retrieve orderline details for order id: " + orderId);
                return new EcrResult { ErrorMessage = "Booking failed for ECR OrderId: " + order.Id + " couldn't retrieve orderline details.", Status = EcrResponseCodes.BookingFailure }; 
            }

            //check site that doesn't support QR Code as all may need it.

            //pull orderlines that need sending to Ecr.
            //var selectedOrderLines = order.OrderLines.Where(x => x.MicroSite);

            if (CurrentSite.NewCKEcrVersionId == (int) EcrVersion.Three)
            {
                var response = SendBookingToEcr3(order);

                //check response is OK
                if (response == null)
                {
                    return new EcrResult { ErrorMessage = "Booking failed for ECR OrderId: " + order.Id, Status = EcrResponseCodes.BookingFailure };
                }

                //check barcode are available
                if (response.Barcodes == null || response.Barcodes.Length < 1)
                {
                    return new EcrResult
                    {
                        ErrorMessage = "Booking failed (no barcode returned for ECR OrderId: " + order.Id,
                        Status = EcrResponseCodes.QrCodeRetrievalFailure
                    };
                }

                order.EcrBookingShortReference = response.TransactionReference;
                CheckoutService.SaveOrder(order);

                Log("Saving external barcodes");
                SaveBarcodes(response, order.OrderNumber);

                return new EcrResult { Status = EcrResponseCodes.BookingSuccess };
            }
            else if(CurrentSite.NewCKEcrVersionId == (int)EcrVersion.One)
            {
                //add web service for old Ecr version

                //make up order booking

                return new EcrResult { Status = EcrResponseCodes.BookingFailure };
            }

            return new EcrResult { Status = EcrResponseCodes.BookingFailure };
        }

        protected BookingResponse SendBookingToEcr3(Order order) {

            var orderLines = order.OrderLines.ToList();

            var availability = EcrServiceHelper.GetAvailabilityFromOrderLines(orderLines);

            var availabilityResponse = EcrService.GetAvailability(availability);

            if(availabilityResponse == null) 
            {
                Log("Availability failed for OrderId: " + order.Id);
                return null;
            }

            if (string.IsNullOrEmpty(availabilityResponse.TransactionReference))
            {
                Log("Availability response returned no transaction ref OrderId: " + order.Id + 
                    System.Environment.NewLine + " error: " + availabilityResponse.ErrorDescription);
                return null;
            }

            var bookingTransactions = EcrServiceHelper.GetBookingTransactionDetails(orderLines, order.Currency.ISOCode);

            var response = EcrService.SubmitBooking(order.OrderNumber, availabilityResponse, bookingTransactions);

            if (response != null && response.Status == (int) EcrResponseStatus.Success) return response;

            Log("Send to Ecr Failed with error " + (response == null ? "Booking process failed " : response.ErrorDescription));
            return null;
        }

        protected void ClearCheckoutCookies()
        {
            AuthenticationService.ExpireCookie(SessionCookieName);
            AuthenticationService.ExpireCookie(BasketCookieName);
            //put session in complete mode
        }

        public void SaveBarcodes(BookingResponse response, int orderNumber)
        {
            try
            {
                var barcodes = response.Barcodes;

                foreach (var barcode in barcodes)
                {
                    var chartUrl = string.Format(GoogleChartUrl, barcode.BarcodeAsText);

                    var imageBytes = ImageService.DownloadImageFromUrl(chartUrl);

                    var ticket =
                        TicketService.GetTicketByProductDimensionUid(barcode.BarcodeDetails[0].ProductDimensionUID);

                    var imageSaveStatus = ImageDbService.GenerateQrImage(orderNumber, ticket.Id.ToString(),
                        imageBytes, MicrositeId);
                }
            }
            catch (Exception ex)
            {
                Log("SaveBarcodes() Failed Ordernumber: " + orderNumber + System.Environment.NewLine + ex.Message);
            }
        }

        public void SendOrderConfirmationEmail(Order order, Session session)
        {
            if (order == null)
                return;

            var eVoucherPage = EnumHelper.GetDescription(EmailTemplatePages.EVoucher);
            var contactData = NotificationService.GetSiteContactData(MicrositeId, eVoucherPage);
            var rootUrl = UrlHelper.GetRootUrl(Request.Url.AbsoluteUri);
            var defaultRootUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var eVoucherLink = (string.IsNullOrEmpty(rootUrl) ? defaultRootUrl : rootUrl) + "/" +
                               string.Format(ConfigurationManager.AppSettings["View.Voucher"], order.Id);

            var bornUrlRoot = string.Format(ConfigurationManager.AppSettings["BornBaseInsecureUrl"], CurrentLanguageId,
                MicrositeId);

            var currency = CurrencyService.GetCurrencyById(session.CurrencyId);

            var request = new OrderConfirmationEmailRequest
            {
                ReceiverFirstname = order.User.Firstname,
                EmailSubject = MakeSubject(order.OrderNumber),
                SenderEmail = "\"BigBus Admin\" <" + contactData.Email + ">",
                ReceiverEmail = order.EmailAddress,
                CityName = MicrositeId,
                LanguageId = CurrentLanguageId,
                OrderNumber = order.OrderNumber.ToString(),
                ViewAndPrintTicketLink = eVoucherLink,
                UserFullName = order.UserName,
                DateOfOrder = LocalizationService.GetLocalDateTime(MicrositeId).ToShortDateString(), //*** format to local date
                OrderTotal = currency.Symbol + order.Total,
                TicketQuantity = order.TotalQuantity.ToString(),
                TermsAndConditionsLink = bornUrlRoot + ConfigurationManager.AppSettings["TermAndCo.Url"],
                PrivacyPolicyLink = bornUrlRoot + ConfigurationManager.AppSettings["Privacy.Url"],
                ContactUsLink = bornUrlRoot + ConfigurationManager.AppSettings["ContactUs.Url"],
                FaqLink = bornUrlRoot + ConfigurationManager.AppSettings["Faqs.Url"],
                DownloadMapLink = bornUrlRoot + ConfigurationManager.AppSettings["RoutMap.Url"],
                AppStoreLink = MakeAppleDownloadUrl(),
                GooglePlayLink = MakeGooglePlayDownloadUrl(),
                CityNumber = ConfigurationManager.AppSettings[string.Format("{0}_Telephone", MicrositeId)],
                CityEmail = ConfigurationManager.AppSettings[string.Format("{0}_Email", MicrositeId)]
            };

            var result = NotificationService.CreateOrderConfirmationEmail(request);

            Log(result);
        }

        private string MakeSubject(int orderNumber)
        {
            return GetTranslation("email_Your_trip_with_BigBus_has_been_booked") + " (" + GetTranslation("email_Order_number") + ": " + orderNumber + ")";
        }

        private string MakeAppleDownloadUrl()
        {
            var appleBaseUrl = ConfigurationManager.AppSettings["AppStore.Url"];
            var languageId = (AppleLanguageMaps)Enum.Parse(typeof(AppleLanguageMaps), CurrentLanguageId, true);

            return string.Format(appleBaseUrl, EnumHelper.GetDescription(languageId));
        }

        private string MakeGooglePlayDownloadUrl()
        {
            var googlePlayBaseUrl = ConfigurationManager.AppSettings["GooglePlay.Url"];
            var languageId = (GooglePlayLanguageMaps)Enum.Parse(typeof(GooglePlayLanguageMaps), CurrentLanguageId, true);

            var lang =
                (CurrentLanguageId.Equals(GooglePlayLanguageMaps.Eng.ToString(),
                    StringComparison.CurrentCultureIgnoreCase))
                    ? string.Empty
                    : "&hl=" + languageId;

            return string.Format(googlePlayBaseUrl, lang);
        }

        public void AddMetas(string metasHtml)
        {
            //add no follow meta
            try
            {
                var master = (SiteMaster)Master;
                if (master != null) master.AddMetas(metasHtml);
            }
            catch
            {
                //ignore
            }
        }

        public Session GetSession()
        {
            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            return AuthenticationService.GetSession(sessionId);
        }

        public Basket GetBasket()
        {
            try
            {
                var basketCookie = AuthenticationService.GetBasketIdFromCookie(BasketCookieName);
                return BasketService.GetBasket(new Guid(basketCookie));
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return null;
            }
        }

        public void DisplayBasketDetails(Basket basket, BasketDisplay ucBasketDisplay, string currencySymbol)
        {
            ucBasketDisplay.AddMoreUrl = ConfigurationManager.AppSettings["BornAddMoreTicketUrl"];
            ucBasketDisplay.ParentPage = this;
            ucBasketDisplay.ShowActionRow = true;
            ucBasketDisplay.TotalString = currencySymbol + basket.Total;

            var itemList = basket.BasketLines.Select(item => new BasketDisplayVm
            {
                TicketName = TicketService.GetTicketById(item.TicketId.ToString()).Name,
                Date = GetTranslation("OpenDayTicket"),
                Quantity = item.TicketQuantity ?? 1,
                Title = item.TicketType.ToString(),
                TotalSummary = currencySymbol + item.LineTotal.ToString()
            }).ToList();

            ucBasketDisplay.DataSource = itemList;
        }

        public List<FrontEndNavigationItem> GetFooterNavigation()
        {
            var navigationItemsCachKey = string.Format("Navigation_Footer_{0}", MicrositeId);

            var navigation = CacheProvider.GetFromCache<List<FrontEndNavigationItem>>(navigationItemsCachKey);

            if (navigation != null) return navigation;

            //Only cache for the normal users
            navigation = NavigationService.GetNavigationBySiteAndSection(MicrositeId, "footer", CurrentLanguageId);

            //cache this for 5 mins
            if (navigation != null && navigation.Count > 0)
            {
                CacheProvider.AddToCache(navigationItemsCachKey, navigation, DateTime.Now.AddMinutes(5));
            }

            return navigation;
        }

        public void LoadMasterValues()
        {
            var master = Master as SiteMaster;

            if (master == null)
                return;

            master.CurrentLanguage = TranslationService.GetLanguage(CurrentLanguageId);
            master.IsMobileSession = false;
            master.MicrositeId = MicrositeId;
            master.HomeUrl = ConfigurationManager.AppSettings["BornHomeTicketUrl"];
        }

        public void RedirectToHomePage()
        {
            Response.Redirect(ConfigurationManager.AppSettings["BornHomeTicketUrl"]);
        }

        public string GetQuovadisImage()
        {
            return @"<div class=""quovadis-wrapper"">
                        <div class=""quovadis"">
                            <a href="""">
                                <img src=""/Content/images/design/quovadis_110x46.gif"" alt=""QuoVadis Secured Site - Click for details"" onclick=""return popUp();"">
                            </a>
                         </div>
                    </div>";
        }

        public string GetQuovadisScripts()
        {
            return
                @"<script type=""text/javascript"" src=""https://siteseal.quovadisglobal.com/scripts/script.js""></script>
                <script type=""text/javascript"">
                    function popUp() {
                        window.open('https://siteseal.quovadisglobal.com/default.aspx?cn=*.bigbustours.com', 'popUp', 'toolbar=no,resizable=yes,scrollbars=yes,location=yes,dependent=yes,status=0,alwaysRaised=yes,left=300,top=200,width=655,height=610');
                        return false;
                    }
                </script>";
        }

    }
}