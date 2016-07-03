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
        private Basket _basket;
        private string _basketId;

        public EcrResponseCodes EcrBookingStatus;
        public IPciApiServiceNoASync PciApiServices { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {           
            LoadBasket();

            //Take session off the checkout and set it in order creation mode
            Log("Putting session in order creation mode basketid:" + _basketId);
            AuthenticationService.PutSessionInOrderCreationMode(CurrentSession);

            //get and process basket pci status
            var returnedStatus = GetBasketPciStatus();

            //delete basket if we get here.
            Log("Sending delete request to PCI web basketid: " + _basketId);
            PciApiServices.DeletePciBasket(_basketId, CurrentLanguageId, SubSite);//send delete message for basket
                   
            //create order and all lines in DB.
            Log("Starting order creation basketid: " + _basketId);
            var newOrder = CheckoutService.CreateOrder(CurrentSession, _basket, returnedStatus, GetClientIpAddress(), CurrentLanguageId, MicrositeId);

            Log("Payment success - Generate barcode");
            GenerateOrderBarcodes(newOrder);

            //send booking to ECR.
            Log("Sending booking to ECR basketid: " + _basketId);
            var result = SendBookingToEcr(newOrder);

            //result from booking must be there.
            if (result.Status != EcrResponseCodes.BookingSuccess )
            {
                JumpToOrderCreationError(GetTranslation("Booking_failed"), " Status: " + result.Status + " Error: " + result.ErrorMessage);
                return;
            }           

            //clear cookie sessions and remove session from checkout mode
            ClearCheckoutCookies();

            Log("Cookies cleared. Sending emails.");

            //Prepare email notifications
            CreateOrderConfirmationEmail(newOrder);

            //Redirect user to order confirmation page or error
            Response.Redirect(string.Format("~/BookingCompleted.aspx?oid={0}", newOrder.Id), false);
        }   
      

        private void LoadBasket()
        {
            //make sure we have a valid session
            if (CurrentSession == null || CurrentSession.BasketId == null || CurrentSession.BasketId == Guid.Empty)
            {
                GoToErrorPage(GetTranslation("Session_Details_NotFound"), "Session not found in Session");
                return;
            }

            _basketId = CurrentSession.BasketId.ToString();

            if (string.IsNullOrEmpty(_basketId))
            {
                _basketId = AuthenticationService.GetBasketIdFromCookie(BasketCookieName);
                GoToErrorPage(GetTranslation("Session_Basket_NotFound"), "Basket Id not found in Session Id:" + _basketId);
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
            CurrentSession.BasketId = null;
            CurrentSession.InOrderCreationProcess = false;
            CurrentSession.AgentUseCustomersAddress = false;
            CurrentSession.AgentFakeUserId = null;
            CurrentSession.AgentIsTradeTicketSale = true;
            CurrentSession.AgentNameToPrintOnTicket = null;

            AuthenticationService.UpdateSession(CurrentSession);

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