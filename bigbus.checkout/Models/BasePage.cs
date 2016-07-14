
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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BookingResponse = bigbus.checkout.EcrWServiceRefV3.BookingResponse;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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
        public IPageContentService PageContentService { get; set; }

        #endregion

        #region private properties

        private MicroSite _currentSite;
        private string _externalSessionId;
        private Session _currentSession;
        private string _currentSessionId;
        private string _currentLanguageId = "eng";
        private string _currentMicrosite = "london";

        #endregion

        #region config properties

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
        public string EmailTemplatePath { get { return ConfigurationManager.AppSettings["EmailTemplatePath"]; } }
        public string LanguageCookieName { get { return ConfigurationManager.AppSettings["Language.CookieName"]; } }
        public string MicrositeCookieName { get { return ConfigurationManager.AppSettings["Microsite.CookieName"]; } }
        public string LiveEcrEndPoint
        {
            get { return ConfigurationManager.AppSettings["LiveEcrEndPoint"]; }
        }
        public string EcrApiKey { get { return ConfigurationManager.AppSettings["EcrApiKey"]; } }
        public int EnvironmentId { get { return (ConfigurationManager.AppSettings["Environment"] != null)? 
            Convert.ToInt32(ConfigurationManager.AppSettings["Environment"]) : (int)Common.Enums.Environment.Local; } }
        public string PayPalCancelUrl { get { return ConfigurationManager.AppSettings["PayPal.CancelURL"]; } }
        public string PayPalSuccessUrl { get { return ConfigurationManager.AppSettings["PayPal.SuccessURL"]; } }

        #endregion

        #region public properties

        public string CurrentLanguageId
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguageId))
                    return _currentLanguageId;

                _currentLanguageId = AuthenticationService.GetCookieValStr(LanguageCookieName);
                return string.IsNullOrEmpty(_currentLanguageId) ? "eng" : _currentLanguageId;
            }
            set
            {
                _currentLanguageId = value;
                AuthenticationService.SetCookie(LanguageCookieName, SessionCookieDomain, _currentLanguageId);
            }
        }

        public string MicrositeId
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentMicrosite))
                    return _currentMicrosite;

                _currentMicrosite = AuthenticationService.GetCookieValStr(MicrositeCookieName);
                return string.IsNullOrEmpty(_currentMicrosite) ? "london" : _currentMicrosite;
            }
            set
            {
                _currentMicrosite = value;
                AuthenticationService.SetCookie(MicrositeCookieName, SessionCookieDomain, _currentMicrosite);
            }
        }

        public string SubSite
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentMicrosite))
                    return _currentMicrosite;

                _currentMicrosite = AuthenticationService.GetCookieValStr(MicrositeCookieName);
                return string.IsNullOrEmpty(_currentMicrosite) ? "london" : _currentMicrosite;
            }
            set
            {
                _currentMicrosite = value;
                AuthenticationService.SetCookie(MicrositeCookieName, SessionCookieDomain, _currentMicrosite);
            }
        } 

        public string ExternalSessionCookieValue
        {
            get { return _externalSessionId; }
        }
        
        public bool SiteIsUs(ISiteService siteService)
        {
            return siteService.GetMicroSiteById(MicrositeId).IsUS;
        }

        public MicroSite CurrentSite
        {
            get { return _currentSite ?? (_currentSite = SiteService.GetMicroSiteById(MicrositeId)); }
        }

        public Session CurrentSession { get { return _currentSession; } }

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

        #endregion

        #region page events

        protected void Page_PreInit(object sender, EventArgs e)
        {
            GetCurrentSession();

            _externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
            _currentLanguageId = AuthenticationService.GetCookieValStr(LanguageCookieName);
            _currentMicrosite = AuthenticationService.GetCookieValStr(MicrositeCookieName);

            var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
            var cp = cpa.ContainerProvider;
            cp.RequestLifetime.InjectProperties(this);
            LoadMasterValues();
        }

        protected void Page_PreRender(object sender, EventArgs eventArgs)
        {
            AddMetaTagsToPage();
        }

        #endregion

        public Session GetCurrentSession()
        {
            if (_currentSession != null)
                return _currentSession;

            _currentSessionId = AuthenticationService.GetCookieValStr(SessionCookieName);

            if (string.IsNullOrEmpty(_currentSessionId))
            {
                _currentSession = AuthenticationService.CreateNewSession(SessionCookieDomain, SessionCookieName);
                _currentSessionId = _currentSession.Id.ToString();
            }
            else
            {
                _currentSession = AuthenticationService.GetSession(_currentSessionId);
            }

            return _currentSession;
        }

        public string GetTranslation(string keyPhrase)
        {
            return TranslationService.TranslateTerm(keyPhrase, CurrentLanguageId);
        }

        public void Log(string message)
        {
            LoggerService.LogItem(message, _currentSessionId);
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
                                               CalculateBarcodeChecksum(barcode.Substring(0, 12)),
                            DateCreated = DateTime.Now
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
                
                //for API 3 everything goes to ECR
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

                //*** For API one, we need to decide (discuss with ECR)
                //if we have more than 1 ticket, city or date do not send to ECR as we will get Barcodes
                var tourOrderLines = selectedOrderLines.Where(x => x.IsTour);
                var orderLines = tourOrderLines as IList<OrderLine> ?? tourOrderLines.ToList();

                var ticketCount = orderLines.Select(x => x.TicketId).Count();
                var cityCount = orderLines.Select(x => x.MicrositeId).Count();

                if (ecrVersionId == (int)EcrVersion.One && ticketCount == 1 && cityCount == 1)
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

        public string MakeAttractionQrCode(Order order, List<OrderLine> ticketOrderLines, string ticketId)
        {
            try
            {
                var filePath = QrCodeDir + order.OrderNumber +
                    ticketId + order.DateCreated.ToString("ddMMyyyy") + ".png";

                var fi = new FileInfo(Server.MapPath(filePath));

                if (!fi.Exists)
                {
                    string qrcode = GetAttractionQrCode(order, ticketOrderLines);

                    if (!qrcode.Trim().Equals(string.Empty))
                    {
                        var imageBytes = ImageService.DownloadImageFromUrl(qrcode);
                        var oimg = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));

                        oimg.Save(fi.FullName);
                        oimg.Dispose();
                    }
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Log("Voucher.aspx.cs => akeAttractionQrCode() failed - orderid: " + order.Id + "  ex: " + ex.Message);
                return string.Empty;
            }
        }

        private readonly Random _rnd = new Random();

        public string GetAttractionQrCode(Order order, List<OrderLine> attractOrderLines)
        {
            if (string.IsNullOrWhiteSpace(order.AuthCodeNumber))
            {
                lock (_rnd)
                {
                    Thread.Sleep(20);
                    string num = string.Empty;

                    for (int i = 0; i < 10; i++)
                        num += _rnd.Next(0, 9);

                    num = num.Substring(0, 10);

                    order.AuthCodeNumber = num;
                    CheckoutService.SaveOrder(order);
                }
            }

            var productCode = string.Empty;

            foreach (var orderLine in attractOrderLines)
            {
                var generatedBarcodes = CheckoutService.GetOrderLineGeneratedBarcodes(orderLine);

                foreach (var orderLineGeneratedBarcode in generatedBarcodes)
                {
                    productCode += orderLineGeneratedBarcode.GeneratedBarcode + "01";
                    var lineprice = Convert.ToInt32(orderLine.TicketCost * 100);
                    productCode += lineprice.ToString("000000");
                }
            }

            var orderTotal = Convert.ToInt32(order.Total * 100);
            var sixDigitOrderTotalString = orderTotal.ToString("000000"); // Again ensure that the string is at least 6 characters long
            var tenDigitOrderNumber = order.OrderNumber.ToString("0000000000"); // This time we need to ensure the string is at least 10 characters long
            var qrCurrencyCode = order.Currency.QrId.ToString("00"); // Ensure it's at least two characters long

            var qrCodeDataString =
                tenDigitOrderNumber +
                order.AuthCodeNumber +
                qrCurrencyCode +
                sixDigitOrderTotalString +
                order.DateCreated.ToString("ddMMyyyy") +
                productCode;

            return string.Format("https://chart.googleapis.com/chart?cht=qr&chs=200x200&chl={0}", Server.UrlEncode(qrCodeDataString));

        }

        public VoucherTicket LoadVoucherTicketWithQrcode(Order order, List<OrderLine> orderLines, Ticket ticket, MicroSite microsite)
        {
            var validTicketName = ticket.Name.ToLower().Contains(microsite.Name.ToLower())
               ? ticket.Name
               : string.Concat(microsite.Name, " ", ticket.Name);

            var attractionMetaData = ticket.ImageMetaDataId != null
                ? ImageDbService.GetMetaData(ticket.ImageMetaDataId.Value.ToString())
                : null;

            var qrCodeImageData =GetOrderImageMetaData(order);

            return
                    new VoucherTicket
                    {
                        UseQrCode = true,
                        OrderLines = orderLines,
                        Ticket = ticket,
                        AttractionImageData = attractionMetaData,
                        ImageData = qrCodeImageData,
                        ValidTicketName = validTicketName
                    };

        }

        public ImageMetaData GetOrderImageMetaData(Order order)
        {
            return ImageDbService.GetImageMetaDataByName(string.Format(Services.Implementation.ImageDbService.QrImageNameFormat, order.OrderNumber,
                 "Ecr"));   
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

        private bool GetTemplate(OrderConfirmationEmailRequest request)
        {
            var siteTemlate = NotificationService.GetSiteEmailTemplate(MicrositeId, CurrentLanguageId);

            if (siteTemlate == null)
            {
                Log("Site template not found BasePage => GetTemplate() id:" + _externalSessionId);
                return false;
            }

            //populate trip advisor and trust pilot variables
            request.TripAdvisorLink = siteTemlate.TripAdvisorLink;
            request.TrustPilotLink = siteTemlate.TrustPilotLink;

            //attempt to get related email template
            var template = NotificationService.GetEmailTemplate(siteTemlate.EmailTemplateId.ToString());

            if(template == null)
            {
                Log("Email template not found BasePage => GetTemplate() id:" + siteTemlate.EmailTemplateId);
                return false;
            }

            var htmlFilePath = Server.MapPath(EmailTemplatePath + template.ContentFile);

            using (var reader = File.OpenText(htmlFilePath)) // Path to your 
            {
                request.HtmlBody = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(request.HtmlBody) || request.HtmlBody.Length < 200)
            {
                Log("Email template return invalid body BasePage => GetTemplate() id:" + siteTemlate.EmailTemplateId);
                return false;
            }
            return true;
        }

        public void CreateOrderConfirmationEmail(Order order)
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

            var currency = CurrencyService.GetCurrencyById(order.CurrencyId.ToString());

            var request = new OrderConfirmationEmailRequest
            {
                ReceiverFirstname = order.User.Firstname,
                EmailSubject = MakeSubject(order.OrderNumber),
                SenderEmail = "\"BigBus Admin\" <" + contactData.Email + ">",
                ReceiverEmail = order.EmailAddress,
                CcEmails = contactData.Email,
                CityName = MicrositeId,
                LanguageId = CurrentLanguageId,
                OrderNumber = order.OrderNumber.ToString(),
                OrderId = order.Id.ToString(),
                ViewAndPrintTicketLink = eVoucherLink,
                UserFullName = order.UserName,
                DateOfOrder = LocalizationService.GetLocalDateTime(MicrositeId).ToString("dd MMM yyyy"), //*** format to local date
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
                CityEmail = ConfigurationManager.AppSettings[string.Format("{0}_Email", MicrositeId)],
                ViewInBrowserLink = BaseUrl + "ViewEmail.aspx?oid=" + order.Id
            };

            var templateSuccessful = GetTemplate(request);

            if (!templateSuccessful)
                return;

            var result = NotificationService.CreateOrderConfirmationEmail(request);

            if (!string.IsNullOrEmpty(result))
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
                returnString.AppendLine(line.Ticket.Name + " - " + line.TicketQuantity + " " + line.TicketType + " ticket(s)<br/>");
            }

            returnString.AppendLine(TranslationService.TranslateTerm("ExpectedTourDate", CurrentLanguageId) + ": " +
                                    expectedTravelDate);

            return returnString.ToString();
        }

        private string MakeSubject(int orderNumber)
        {
            return GetTranslation("email_Your_trip_with_BigBus_has_been_booked") + " (" + GetTranslation("email_Order_number") + ": " + orderNumber + ")";
        }

        protected string MakeAppleDownloadUrl()
        {
            var appleBaseUrl = ConfigurationManager.AppSettings["AppStore.Url"];
            var languageId = (AppleLanguageMaps)Enum.Parse(typeof(AppleLanguageMaps), CurrentLanguageId, true);

            return string.Format(appleBaseUrl, EnumHelper.GetDescription(languageId));
        }

        protected string MakeGooglePlayDownloadUrl()
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
           
            ucBasketDisplay.TotalString = currencySymbol + basket.Total.ToString("F");

            var itemList = basket.BasketLines.Select(item => new BasketDisplayVm
            {
                TicketName = BasketTicketName(item.TicketId.ToString()),
                Date = GetTranslation("OpenDayTicket"),
                Quantity = item.TicketQuantity ?? 1,
                Title = item.TicketType.ToString(),
                TotalSummary = currencySymbol + item.LineTotal.ToString()
            }).ToList();

            ucBasketDisplay.DataSource = itemList;
        }

        public string BasketTicketName(string ticketId)
        {
            var ticket = TicketService.GetTicketById(ticketId);
            return ticket.Name + " - " + ticket.MicroSiteId;
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

        public void ReLoadMasterValues(string languageId, string micrositeId)
        {
            var master = Master as SiteMaster;

            if (master == null)
                return;

            master.CurrentLanguage = TranslationService.GetLanguage(languageId);
            master.IsMobileSession = false;
            master.MicrositeId = micrositeId;
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

        #region IHasCMSMetaTagHooks Members

        public virtual string DefaultPageDescription
        {
            get
            {
                return
                    "Big Bus Sightseeing Tours have been designed to provide you with a flexible approach to city discovery; allowing you to select the attractions and places of interest that appeal to you, whilst affording time to explore at leisure.";
            }
        }

        public virtual string DefaultPageKeywords
        {
            get { return "Big, Bus, Sightseeing, Tours"; }
        }

        public virtual string DefaultPageTitle
        {
            get { return "Big Bus Tours"; }
        }
        
        #endregion

        #region AddMetaTagsToPage

        public void AddMetaTagsToPage()
        {
            if (!AutoLoadChameleonMetaTags) return;

            var pageType = CmsPageType;
            var regex = new Regex(@"\\[uU]([0-9A-Fa-f]{4})", RegexOptions.IgnoreCase);
            var pageIdentifier = regex.Replace(CmsPageIdentifier, match => char.ConvertFromUtf32(Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)));
                
            var desc = DefaultPageDescription;
            var keywords = DefaultPageKeywords;
            var title = DefaultPageTitle;


            var tags = PageContentService.GetPageMetaTags(pageType, pageIdentifier);

            foreach (var tag in tags)
            {
                switch (tag.MetaTag)
                {
                    case "Keywords":
                        keywords = tag.Value;
                        break;
                    case "Description":
                        desc = tag.Value;
                        break;
                    case "Title":
                        title = tag.Value;
                        break;
                    default:
                        if (Page.Header != null)
                        {
                            Page.Header.Controls.Add(
                                new LiteralControl(string.Format("<meta name=\"{0}\" content=\"{1}\" />", tag.MetaTag, tag.Value)));
                        }
                        break;
                }
            }

            if (Page.Header == null)
            {
                return;
            }

            var arr = new Control[Page.Header.Controls.Count];

            if (Page.Header != null)
            {
                Page.Header.Controls.CopyTo(arr, 0);

                if (InjectViewportMetaTag)
                {
                    Page.Header.Controls.Add(
                        new LiteralControl(
                            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1, minimum-scale=1\">"));
                }
            }

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                var theControls =
                    arr
                        .Where(a => a.GetType() == typeof(HtmlMeta))
                        .Select(a => ((HtmlMeta)a)).Where(a => a.Attributes["Name"] == "Keywords")
                        .ToList();

                foreach (var theControl in theControls)
                {
                    Page.Header.Controls.Remove(theControl);
                }

                Page.Header.Controls.Add(new LiteralControl(string.Format("<meta name=\"Keywords\" content=\"{0}\" />", keywords)));
            }

            if (!string.IsNullOrWhiteSpace(desc))
            {
                var theControls =
                    arr
                        .Where(a => a.GetType() == typeof(HtmlMeta))
                        .Select(a => ((HtmlMeta)a)).Where(a => a.Attributes["Name"] == "Description")
                        .ToList();

                foreach (var theControl in theControls)
                {
                    Page.Header.Controls.Remove(theControl);
                }

                Page.Header.Controls.Add(new LiteralControl(string.Format("<meta name=\"Description\" content=\"{0}\" />", desc)));
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                Page.Title = title;
            }
        }

        protected virtual bool AutoLoadChameleonMetaTags { get { return true; } }
        public virtual string CmsPageIdentifier { get { return "Default"; } }
        public virtual string CmsPageType { get { return "System"; } }
        public virtual bool InjectViewportMetaTag { get { return false; } }

        #endregion

    }
}