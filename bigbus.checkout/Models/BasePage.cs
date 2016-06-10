
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
using Common.Helpers;
using Common.Model;
using System.Data;
using Common.Model.Interfaces;
using BigBusWebsite.controls.SharedLayout;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using bigbus.checkout.data.PlainQueries;
using bigbus.checkout.Ecr1ServiceRef;
using BookingResponse = bigbus.checkout.EcrWServiceRefV3.BookingResponse;

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
        public IEcrApi3ServiceHelper Ecr3ServiceHelper { get;set; }

        #endregion

        private MicroSite _currentSite;
        private string _externalSessionId;

        public string ExternalBasketCookieName { get { return ConfigurationManager.AppSettings["External.Basket.CookieName"]; } }
        public string SessionCookieName { get { return ConfigurationManager.AppSettings["Session.CookieName"]; } }
        public string SessionCookieDomain { get { return ConfigurationManager.AppSettings["Session.CookieDomain"]; } }
        public string PciApiDomain{ get { return ConfigurationManager.AppSettings["PciWebsite.ApiDomain"]; } }
        public string PciDomain { get { return ConfigurationManager.AppSettings["PciWebsite.Domain"]; } }
        public string PciLandingPagePath { get { return ConfigurationManager.AppSettings["PciWebsite.LandingPagePath"]; } }
        public string BasketCookieName { get { return ConfigurationManager.AppSettings["Basket.CookieName"]; } }
        public string GoogleChartUrl { get { return ConfigurationManager.AppSettings["GoogleChartUrl"]; } }
        public string BornBaseUrl { get { return ConfigurationManager.AppSettings["BornBaseUrl"]; } }
        public string BaseUrl { get { return ConfigurationManager.AppSettings["BaseUrl"]; } }
        public string BarCodeDir { get { return ConfigurationManager.AppSettings["BarCodeDir"]; } }
        public string QrCodeDir { get { return ConfigurationManager.AppSettings["QrCodeDir"]; } }

        public string LiveEcrEndPoint
        {
            get { return ConfigurationManager.AppSettings["LiveEcrEndPoint"]; }
        }
        public string EcrApiKey { get { return ConfigurationManager.AppSettings["EcrApiKey"]; } }
        public int EnvironmentId { get { return (ConfigurationManager.AppSettings["Environment"] != null)? 
            Convert.ToInt32(ConfigurationManager.AppSettings["Environment"]) : (int)Common.Enums.Environment.Local; } }

        public string CurrentLanguageId { get { return "eng"; } } //***replace with function
        public string MicrositeId { get { return "london"; } } //*** get from url as in function
        public string SubSite { get { return "london"; } }  //*** work out from old code

        public bool SiteIsUs(ISiteService siteService)
        {
            return siteService.GetMicroSiteById(MicrositeId).IsUS;
        }

        public MicroSite CurrentSite
        {
            get { return _currentSite ?? (_currentSite = SiteService.GetMicroSiteById(MicrositeId)); }
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
            _externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
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
            LoggerService.LogItem(message, _externalSessionId);
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

                if (orderLines == null || orderLines.Count <= 0) return;

                foreach (var orderline in orderLines)
                {
                    var orderLineGenerateBarCodes = CheckoutService.GetOrderLineGeneratedBarcodes(orderline);

                    for (var y = 0; y < orderline.TicketQuantity; y++)
                    {
                        if (orderLineGenerateBarCodes != null && orderLineGenerateBarCodes.Count != 0) continue;

                        var ticket = (orderline.Ticket == null && orderline.TicketId != null)? 
                            TicketService.GetTicketById(orderline.TicketId.Value.ToString())
                            : orderline.Ticket;

                        var barcode = BarcodeService.GetNextBarcode(ticket, orderline.TicketType).Substring(0, 12);

                        var orderlineGeneratedBarcode = new OrderLineGeneratedBarcode
                        {
                            OrderLineId = orderline.Id,
                            GeneratedBarcode = barcode +
                                               CalculateBarcodeChecksum(barcode.Substring(0, 12))
                        };

                        CheckoutService.SaveOrderLineBarCode(orderlineGeneratedBarcode);
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
            var queryVersionGroups =
               from detail in orderLineDetails
               group detail by detail.NewCheckoutVersionId into versionGroup
               orderby versionGroup.Key
               select versionGroup;

            if (!queryVersionGroups.Any())
            {
                return new EcrResult {ErrorMessage = "Could not find ECR Settings for Orderlines orderid:" + orderId, Status = EcrResponseCodes.BookingFailure};
            }

            foreach (var versionGroup in queryVersionGroups)
            {
                var ecrVersionId = versionGroup.Key;
                Log("Processing ecr versionid " + ecrVersionId + " orderid " + order.Id);

                var selectedOrderLines = order.OrderLines.Where(a => versionGroup.ToList().Any(x =>
                    x.OrderLineId.Equals(a.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();
                
                if (ecrVersionId == (int)EcrVersion.Three)
                {
                    var response = Ecr3ServiceHelper.SendBookingToEcr3(order, selectedOrderLines);
                    
                    //check response is OK
                    if (response == null || response.Status != 0)
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
                    SaveBarcodes(response, order);

                    Log("BasePage => SendBookingToEcr() - Booking sent to Ecr API3");
                    
                }
                else if (ecrVersionId == (int)EcrVersion.One)
                {
                    Log("BasePage SendBookingToEcr => sending booking to ECR1");

                    var result = Ecr3ServiceHelper.SendBookingToEcr1(order, selectedOrderLines, versionGroup.ToList());

                    Log("BasePage SendBookingToEcr => booking send to ECR 1 result:" + result);

                    if(!result)
                        Log(Ecr3ServiceHelper.GetLastBookingErrors());
                }

            }
           
            return new EcrResult { Status = EcrResponseCodes.BookingSuccess };
        }

        protected void ClearCheckoutCookies()
        {
            AuthenticationService.ExpireCookie(SessionCookieName);
            AuthenticationService.ExpireCookie(BasketCookieName);
            //put session in complete mode
        }

        public void SaveBarcodes(BookingResponse response,Order order)
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

                    var imageSaveStatus = ImageDbService.GenerateQrImage(order.OrderNumber, ticket.Id.ToString(),
                        imageBytes, MicrositeId);
                }

                //***also check for ecr1 if there is order ascurl and save old qrcode
                if (!string.IsNullOrEmpty(order.CentinelAcsurl))
                {
                    var chartUrl = string.Format(GoogleChartUrl, order.CentinelAcsurl);

                    var imageBytes = ImageService.DownloadImageFromUrl(chartUrl);
                    
                    var imageSaveStatus = ImageDbService.GenerateQrImage(order.OrderNumber,imageBytes, MicrositeId);

                    if (imageSaveStatus != QrImageSaveStatus.Success && imageSaveStatus != QrImageSaveStatus.ImageDataExist)
                    {
                        Log("Image create failed for order " + order.OrderNumber);
                    }
                }

            }
            catch (Exception ex)
            {
                Log("SaveBarcodes() Failed Ordernumber: " + order.OrderNumber + System.Environment.NewLine + ex.Message);
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
                TicketDetails = FormatOrderDetails(order),
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

        private string FormatOrderDetails(Order order)
        {
            var orderLines = order.OrderLines;
            if (orderLines == null)
            {
                Log("No order lines for orderid: " + order.Id);
                return string.Empty;
            }

            var returnString = new StringBuilder();
            var expectedTravelDate = (order.User.ExpectedTravelDate != null)
                ? order.User.ExpectedTravelDate.Value.ToString("ddmmyy")
                : string.Empty;

            foreach (var line in orderLines)
            {
                line.Ticket = line.Ticket ?? TicketService.GetTicketById(line.TicketId.ToString());
                returnString.AppendLine(line.Ticket.Name + " - " + line.TicketQuantity + " " + line.TicketType + " ticket(s) - ");
            }

            returnString.AppendLine(TranslationService.TranslateTerm("ExpectedTourDate", CurrentLanguageId) + ": " +
                                    expectedTravelDate);

            return returnString.ToString();
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
            master.HomeUrl = BornBaseUrl + ConfigurationManager.AppSettings["BornHomeTicketUrl"];
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