using System;
using System.Configuration;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Enums;
using pci = Common.Model.Pci;
using Services.Infrastructure;
using Basket = bigbus.checkout.data.Model.Basket;
using bigbus.checkout.EcrWServiceRefV3;
using Common.Helpers;
using Common.Model;

namespace bigbus.checkout
{
    public partial class BookingSuccess : BasePage
    {
        private Session _session;
        private Basket _basket;
        private string _basketId;

        public EcrResponseCodes EcrBookingStatus;
        public IPciApiServiceNoASync PciApiServices { get; set; }
        public INotificationService NotificationService {get;set;}
        public ILocalizationService LocalizationService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Load Session & Basket
            LoadSession();
            LoadBasket();

            //Take session off the checkout and set it in order creation mode
            Log("Putting session in order creation mode basketid:" + _basketId);
            AuthenticationService.PutSessionInOrderCreationMode(_session);

            //get and process basket pci status
            var returnedStatus = GetBasketPciStatus();

            //delete basket if we get here.
            Log("Sending delete request to PCI web basketid: " + _basketId);
            PciApiServices.DeletePciBasket(_basketId, CurrentLanguageId, SubSite);//send delete message for basket
                   
            //create order and all lines in DB.
            Log("Starting order creation basketid: " + _basketId);
            var newOrder = CheckoutService.CreateOrder(_session, _basket, returnedStatus, GetClientIpAddress(), CurrentLanguageId, MicrositeId);

            //send booking to ECR.
            Log("Sending booking to ECR basketid: " + _basketId);
            var result = SendBookingToEcr(newOrder);

            //result from booking must be there.
            if (result == null)
            {
                JumpToOrderCreationError("Booking_failed", "Booking failed for ECR basketId: " + _basketId);
            }

            //make sure we have barcode returned
            if (result.Barcodes.Length < 1)
            {
                JumpToOrderCreationError("Booking_failed", "Booking failed (no barcode returned for ECR basketId: " + _basketId 
                    + System.Environment.NewLine + " message: " + result.ErrorDescription);
            }

            newOrder.EcrBookingShortReference = result.TransactionReference;
            CheckoutService.SaveOrder(newOrder);

            Log("Saving external barcodes");
            SaveBarcodes(result, newOrder.OrderNumber);

            //CreateQrImages(result, newOrder);

            //clear cookie sessions and remove session from checkout mode
            ClearCheckoutCookies();

            //Prepare email notifications
            SendOrderConfirmationEmail(newOrder);

            //Redirect user to order confirmation page or error
            Response.Redirect(string.Format("~/BookingCompleted.aspx?oid={0}", newOrder.Id));
        }    

        private void SendOrderConfirmationEmail(Order order)
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

            var currency = CurrencyService.GetCurrencyById(_session.CurrencyId);

            var request = new OrderConfirmationEmailRequest
            {
               EmailSubject =  MakeSubject(order.OrderNumber),
                SenderEmail = contactData.Email,
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
                PrivacyPolicyLink = bornUrlRoot + ConfigurationManager.AppSettings["Privac.Url"],
                AppStoreLink = MakeAppleDownloadUrl(),
                GooglePlayLink = MakeGooglePlayDownloadUrl(),
                CityNumber = ConfigurationManager.AppSettings[string.Format("{0}_Telephone", MicrositeId)],
                CityEmail = ConfigurationManager.AppSettings[string.Format("{0}_Email", MicrositeId)]
            };

            var result = NotificationService.CreateOrderConfirmationEmail(request);
        }

        private string MakeSubject(int orderNumber)
        {
            return GetTranslation("Booking_Collect_Ticket_details") + " (" + GetTranslation("email_Order_number") + ": " + orderNumber + ")";
        }

        private string MakeAppleDownloadUrl()
        {
            var appleBaseUrl = ConfigurationManager.AppSettings["AppStore.Url"];
            var languageId = (AppleLanguageMaps) Enum.Parse(typeof (AppleLanguageMaps), CurrentLanguageId);

            return string.Format(appleBaseUrl, EnumHelper.GetDescription(languageId));
        }

        private string MakeGooglePlayDownloadUrl()
        {
            if (CurrentLanguageId.Equals(GooglePlayLanguageMaps.Eng.ToString(),
                StringComparison.CurrentCultureIgnoreCase))
                return string.Empty;

            var googlePlayBaseUrl = ConfigurationManager.AppSettings["GooglePlay.Url"];
            var languageId = (GooglePlayLanguageMaps) Enum.Parse(typeof (GooglePlayLanguageMaps), CurrentLanguageId);

            return string.Format(googlePlayBaseUrl, "&hl=" + languageId);
        }

        private void LoadSession()
        {
            _session = GetSession();

            if (_session != null) return;

            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            GoToErrorPage(GetTranslation("Session_Details_NotFound"), "Session Not found id:" + sessionId);
        }

        private void LoadBasket()
        {
            _basketId = (_session.BasketId == null || _session.BasketId == Guid.Empty) ? string.Empty : _session.BasketId.ToString();

            if (string.IsNullOrEmpty(_basketId))
            {
                _basketId = AuthenticationService.GetBasketIdFromCookie(BasketCookieName);
                GoToErrorPage(GetTranslation("Session_Basket_NotFound"), "Basket Id not found in Session");
                return;
            }

            Log("Retrieving basket with id:" + _basketId);
            _basket = BasketService.GetBasket(new Guid(_basketId));    
        }

        private pci.BasketStatus GetBasketPciStatus()
        {
            //Get basket status from PCI website
            Log("Calling pci to get basket status. basketid:" + _basketId);
            var basketStatus = PciApiServices.GetBasketPciStatus(_basketId, CurrentLanguageId, SubSite);

            Log("PCI web request returned status of basket ");
            if (basketStatus == null || basketStatus.Status != ReturnStatus.Success)
            {
                GoToErrorPage(GetTranslation("Session_Basket_NotFound"), "Basket bad status from PCI basketid:" + _basketId);
                return null;
            }

            var returnedStatus = basketStatus.ReturnObject as pci.BasketStatus;
            Log(string.Format("PCI returned status {0} basketid: {1}", (int)PciBasketStatus.Success, _basketId));

            //Status must be 4(success) otherwise jump out and show error
            if (returnedStatus == null || returnedStatus.status.code == (int) PciBasketStatus.Success)
                return returnedStatus;

            Log("Invalid PCI code for basket returned : " + (returnedStatus.status == null ? "" : "Code=" + returnedStatus.status.code));
            GoToErrorPage(GetTranslation("Booking_failed"), "Basket status object casting crashed. basketid:" + _basketId);
            return null;
        }

       

        private void JumpToOrderCreationError(string message, string logMessage)
        {
            _session.BasketId = null;
            _session.InOrderCreationProcess = false;
            _session.AgentUseCustomersAddress = false;
            _session.AgentFakeUserId = null;
            _session.AgentIsTradeTicketSale = true;
            _session.AgentNameToPrintOnTicket = null;

            AuthenticationService.UpdateSession(_session);

            if (Response.Cookies[BasketCookieName] != null)
            {
                Response.Cookies[BasketCookieName].Expires = DateTime.Now.AddDays(-1);
            }

            GoToErrorPage(message, logMessage);
        }

        private void GoToErrorPage(string message, string logMessage)
        {
            Log(logMessage);
            Response.Redirect("~/Error/ExternalAPiError/" + Server.UrlEncode(message));
        }
    }
}