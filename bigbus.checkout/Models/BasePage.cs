
using System;
using System.Configuration;
using System.Linq;
using Services.Infrastructure;
using Autofac.Integration.Web;
using System.Web;
using Autofac;
using bigbus.checkout.Helpers;
using bigbus.checkout.data.Model;
using Common.Model.Ecr;
using System.Collections.Generic;
using Common.Enums;

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

        #endregion

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

        protected string CurrentLanguageId { get { return "eng"; } } //***replace with function
        protected string MicrositeId { get { return "london"; } } //*** get from url as in function
        protected string SubSite { get { return "london"; } }  //*** work out from old code

        protected bool SiteIsUs(ISiteService siteService)
        {
            return siteService.GetMicroSiteById(MicrositeId).IsUS;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
            var cp = cpa.ContainerProvider;
            cp.RequestLifetime.InjectProperties(this);
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

        protected BookingResult SendBookingToEcr(Order order)
        {
            EcrServiceHelper ecrHelper;

            switch (EnvironmentId)
            {
                case (int)Common.Enums.Environment.Live:
                    ecrHelper = new EcrServiceHelper(EcrApiKey, LiveEcrEndPoint, TicketService, SiteService);
                    break;
                case (int)Common.Enums.Environment.Local:
                case (int)Common.Enums.Environment.Staging:
                    ecrHelper = new EcrServiceHelper(EcrApiKey, TicketService, SiteService);
                    break;
                default:
                    Log("Invalid environment specified for ECR Api 2. EnvironmentId: " + EnvironmentId);
                    throw new Exception("Invalid environment specified for ECR Api 2");
            }

            var response = ecrHelper.SubmitBooking(order);

            /*** For testing I comment this as ECR API is not working correctly - uncomment before going live.
            if (response == null || response.TransactionStatus.Status != 0)
            {
                Log("Send to Ecr Failed with error " + (response == null ? "Booking process failed " : response.TransactionStatus.Description));
                EcrBookingStatus = EcrResponseCodes.BookingFailure;
                return false;
            }

            order.EcrBookingBarCode = response.BookingBarcode;
            order.EcrBookingShortReference = response.BookingShortReference;
            order.EcrSupplierConfirmationNumber = response.SupplierConfirmationNumber;
            */

            return new BookingResult
            {
                SupplierConfirmation = "66666666-e06d-4309-845a-daae9a106666",
                BookingShortReference = "6666666666",
                Barcodes = new List<Barcode>
                {
                    new Barcode {TicketId = "14D9F26C-2062-4654-9B79-01EBA0EC3930", Code = "6666666636307002005800201511191175" },
                    new Barcode {TicketId = "14D9F26C-2062-4654-9B79-01EBA0EC3930", Code = "CH1009950010026001175AD100994001006666" }
                }
            };

            //order.EcrBookingBarCode = "6666666636307002005800201511191175CH1009950010026001175AD100994001006666";
           // order.EcrBookingShortReference = "6666666666";
           // order.EcrSupplierConfirmationNumber = "66666666-e06d-4309-845a-daae9a106666";

            //CheckoutService.SaveOrder(order);

        }

        protected void ClearCheckoutCookies()
        {
            AuthenticationService.ExpireCookie(SessionCookieName);
            AuthenticationService.ExpireCookie(BasketCookieName);
            //put session in complete mode
        }

        protected void CreateQrImages(BookingResult result, Order order)
        {
            //make the QR code Image
            foreach (var code in result.Barcodes)
            {
                var chartUrl = string.Format(GoogleChartUrl, code.Code);

                //get image from google
                Log("Downloading QR Image from google basketid: " + order.BasketId);
                var imageBytes = ImageService.DownloadImageFromUrl(chartUrl);

                //store image to basket
                Log("Create image QR Code in DB details basketid: " + order.BasketId);
                var status = ImageDbService.GenerateQrImage(order, imageBytes, MicrositeId);

                //check if image has been stored successfully
                if (status == QrImageSaveStatus.Success)
                {
                    Log("QR Image Created successfully created orderid:" + order.Id);
                }
                else
                {
                    Log("QR Image Failed to create orderid:" + order.Id + " code " + code.Code);
                    //GoToErrorPage(GetTranslation("Basket_BadPci_Status"), "Basket status object casting crashed. basketid:" + _basketId);
                }

            }

        }
    }
}