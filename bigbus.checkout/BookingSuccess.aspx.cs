using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using Common.Enums;
using Common.Model;
using Services.Implementation;
using pci = Common.Model.Pci;
using Services.Infrastructure;
using Basket = bigbus.checkout.data.Model.Basket;

namespace bigbus.checkout
{
    public partial class BookingSuccess : BasePage
    {
        private Session _session;
        private Basket _basket;
        private string _basketId;

        public EcrResponseCodes EcrBookingStatus;       
        public IPciApiServiceNoASync PciApiServices { get; set; }
        public ICheckoutService CheckoutService { get; set; }
        public IImageDbService ImageDbService { get; set; }
        public IImageService ImageService { get; set; }

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
            //order must be there.
            if (!result)
            {
                JumpToOrderCreationError("Booking failed", "Booking failed for ECR basketId: " + _basketId);
            }

            //make the QR code Image
            var chartUrl = string.Format(GoogleChartUrl, newOrder.EcrBookingBarCode);
            
            //get image from google
            Log("Downloading QR Image from google basketid: " + _basketId);
            var imageBytes = ImageService.DownloadImageFromUrl(chartUrl);

            //store image to basket
            Log("Create image QR Code in DB details basketid: " + _basketId);
            var status = ImageDbService.GenerateQrImage(newOrder, imageBytes, MicrositeId);

            //check if image has been stored successfully
            if (status == QrImageSaveStatus.Success)
            {
                Log("QR Image Created successfully basketid:" + _basketId);
            }
            else
            {
                GoToErrorPage(GetTranslation("Basket_BadPci_Status"), "Basket status object casting crashed. basketid:" + _basketId);
            }

            //clear cookie sessions and remove session from checkout mode
            ClearCheckoutCookies();

            //Prepare email notifications

            //Redirect user to order confirmation page or error
            Response.Redirect(string.Format("~/Checkout/Completed/{0}", CurrentLanguageId));
        }

        private void ClearCheckoutCookies()
        {
            AuthenticationService.ExpireCookie(SessionCookieName);
            AuthenticationService.ExpireCookie(BasketCookieName);
            //put session in complete mode
        }

        private void LoadSession()
        {
            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            Log("Checkout Success started. sessionid: " + sessionId);

            _session = AuthenticationService.GetSession(sessionId);

            if (_session == null)
            {
                GoToErrorPage(GetTranslation("Empty_Session_NotFound"), "Session Not found id:" + sessionId);
            }
        }

        private void LoadBasket()
        {
            _basketId = (_session.BasketId == null || _session.BasketId == Guid.Empty) ? string.Empty : _session.BasketId.ToString();

            if (string.IsNullOrEmpty(_basketId))
            {
                _basketId = AuthenticationService.GetBasketIdFromCookie(BasketCookieName);
                GoToErrorPage(GetTranslation("BasketId_NotFound"), "Basket Id not found in Session");
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
                GoToErrorPage(GetTranslation("Basket_NotFound"), "Basket bad status from PCI basketid:" + _basketId);
                return null;
            }

            var returnedStatus = basketStatus.ReturnObject as pci.BasketStatus;
            Log(string.Format("PCI returned status {0} basketid: {1}", (int)PciBasketStatus.Success, _basketId));

            //Status must be 4(success) otherwise jump out and show error
            if (returnedStatus == null || returnedStatus.status.code == (int) PciBasketStatus.Success)
                return returnedStatus;

            Log("Invalid PCI code for basket returned : " + (returnedStatus.status == null ? "" : "Code=" + returnedStatus.status.code));
            GoToErrorPage(GetTranslation("Basket_BadPci_Status"), "Basket status object casting crashed. basketid:" + _basketId);
            return null;
        }

        private bool SendBookingToEcr(Order order)
        {
            EcrServiceHelper ecrHelper;
            
            switch (EnvironmentId)
            {
                case (int) Common.Enums.Environment.Live:
                    ecrHelper = new EcrServiceHelper(EcrApiKey, LiveEcrEndPoint,TicketService, SiteService);
                    break;
                case (int) Common.Enums.Environment.Local:
                case (int) Common.Enums.Environment.Staging:
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

            order.EcrBookingBarCode = "6666666636307002005800201511191175CH1009950010026001175AD100994001006666";
            order.EcrBookingShortReference = "6666666666";
            order.EcrSupplierConfirmationNumber = "66666666-e06d-4309-845a-daae9a106666";

            CheckoutService.SaveOrder(order);

            return true;
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
            Response.Redirect("BookingOrderCreationError.aspx?msg=" + Server.UrlEncode(message));
        }
    }
}