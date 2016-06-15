using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Model;
using Common.Model.PayPal;
using Services.Implementation;
using Services.Infrastructure;
using Common.Enums;

namespace bigbus.checkout
{
    public partial class BookingAddressPayPal : BasePage
    {
        private Session _session;
        private Basket _basket;
        protected string TotalSummary { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Log("BookingAddressPayPal => Page_Load() started.");
            _session = GetSession();
            _basket = GetBasket();
            GetPayerDetails();

            //handle validation make sure there is basket and session available.
            var currency = CurrencyService.GetCurrencyById(_basket.CurrencyId.ToString());
            TotalSummary = currency.Symbol + _basket.Total;
            DisplayBasketDetails(_basket, ucBasketDisplay, currency.Symbol);
        }

        protected void CompletePaypalCheckout(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var orderId = string.Empty;
            var bCheckoutCompleted = false;

            try
            {
                // end create new user
                var newUser = CreateUser();

                _session.InCheckoutProccess = false;
                _session.InOrderCreationProcess = true;

                AuthenticationService.UpdateSession(_session);

                if (_session.BasketId == null)
                {
                    Log("Invalid or missing basket in session. Id:" + _session.Id);
                    return;
                }

                //_basket = BasketService.GetBasket(_session.BasketId.Value);
                
                var isoCurrencyCode = CurrencyService.GetCurrencyIsoCodeById(_session.CurrencyId);

                Log("Paypal log- payment to take next, next is order");

                var payPalReturn = PaypalService.ConfirmPayment((_basket.Total.ToString(CultureInfo.InvariantCulture)), isoCurrencyCode, _session.PayPalToken, _session.PayPalPayerId);

                Log("Paypal log- payment taken, next is order, paypal return status: " + payPalReturn.ErrorMessage);

                _session.PayPalOrderId = payPalReturn.Transaction_Id;
                 
                AuthenticationService.UpdateSession(_session);

                if (payPalReturn.IsError)
                {
                    throw new Exception(payPalReturn.ErrorMessage);
                }

                var order = CheckoutService.CreateOrderPayPal(_session, _basket, newUser, GetClientIpAddress(), CurrentLanguageId,
                    MicrositeId);

                orderId = order.Id.ToString();

                CheckoutService.CreateAddressPaypal(order, _session, newUser);

                //send booking to ECR.
                Log("Sending booking to ECR basketid: " + _basket.Id);
                var result = SendBookingToEcr(order);

                //result from booking must be there.
                if (result == null)
                {
                    JumpToOrderCreationError("Booking_failed", result.ErrorMessage);
                    return;
                }

                //clear cookie sessions and remove session from checkout mode
                ClearCheckoutCookies();

                //Prepare email notifications
                CreateOrderConfirmationEmail(order);

                bCheckoutCompleted = true;
            }
            catch (Exception ex)
            {
                Log("Paypal Payment Error: " + ex.Message);
                bCheckoutCompleted = false;
            }
            finally
            {
                UnlockSessionFromOrderCreationLock(_session);
                Log("PayPal Payment - Session unlocked");
            }

            //Redirect user to order confirmation page or error
            Response.Redirect(bCheckoutCompleted
                ? string.Format("~/BookingCompleted.aspx?oid={0}", orderId)
                : @"~/Error/PayPalProcessingError/standard");
        }

      
        private void UnlockSessionFromOrderCreationLock(Session session)
        {
            session.BasketId = null;
            session.InOrderCreationProcess = false;
            session.AgentFakeUserId = null;
            session.AgentIsTradeTicketSale = true;
            session.AgentNameToPrintOnTicket = null;
            // paypal to set null values

            session.PayPalToken = null;
            session.PayPalPayerId = null;
            session.PayPalOrderId = null;

            AuthenticationService.UpdateSession(session);
            AuthenticationService.ExpireCookie(BasketCookieName);
            AuthenticationService.ExpireCookie(SessionCookieName);
            AuthenticationService.ExpireCookie(ExternalBasketCookieName);
        }

        private void DisplayError(string message, string logMessage)
        {
            ltError.Text = message;
            Log(logMessage);
            dvErrorSummary.Visible = true;
        }

        private User CreateUser()
        {
            var customerSession = GetSession();
            Log("Creating User Email: " + ucUserDetails.Email);

            var customer = new Customer
            {
                Title = ucUserDetails.UserTitle,
                Firstname = ucUserDetails.FirstName,
                Lastname = ucUserDetails.LastName,
                Email = ucUserDetails.Email,
                AddressLine1 = ucUserDetails.Address1,
                AddressLine2 = ucUserDetails.Address2,
                City = ucUserDetails.Town,
                PostCode = ucUserDetails.PostCode,
                CountryId = ucUserDetails.Country,
                StateProvince = ucUserDetails.State,
                LanguageId = CurrentLanguageId,
                CurrencyId = new Guid(customerSession.CurrencyId),
                MicroSiteId = MicrositeId,
                Authorised = false,
                ReceiveNewsletter = ucUserDetails.Subscribed
            };

            var user = UserService.CreateCustomer(customer);

            if (customer.Id != Guid.Empty) return user;

            DisplayError(GetTranslation("FailedToCreateUser"), "User creation failed.");
            return null;
        }
        
        private void GetPayerDetails()
        {
            Log("BookingAddressPayPal => GetPayerDetails() - started.");
            
            if(_session == null)
                _session = GetSession();
            
            Log("BookingAddressPayPal => GetPayerDetails() - Confirming Checkout Details.");
            var paypalDetails = PaypalService.ConfirmCheckoutDetails(_session.PayPalToken);

            Log("Updating Paypal Payment details");
            _session.PayPalPayerId = paypalDetails.PayPalReturnUserInfo.Payer_Id;
            _session.PayPalOrderId = paypalDetails.Transaction_Id;
            AuthenticationService.UpdateSession(_session);

            if (paypalDetails.PayPalReturnUserInfo.Firstname != null)
            {
                //populate user details form. and update address session details.
                PopulateCustomerDetails(paypalDetails);
            }
        }

        private void PopulateCustomerDetails(PayPalReturn paypalDetails)
        {
            if ((paypalDetails == null) || paypalDetails.PayPalReturnUserInfo == null) return;

            if (paypalDetails.PayPalReturnUserInfo.Firstname != null)
                ucUserDetails.FirstName = paypalDetails.PayPalReturnUserInfo.Firstname;

            if (paypalDetails.PayPalReturnUserInfo.Lastname != null)
                ucUserDetails.LastName = paypalDetails.PayPalReturnUserInfo.Lastname;

            if (paypalDetails.PayPalReturnUserInfo.Email != null)
                ucUserDetails.Email = paypalDetails.PayPalReturnUserInfo.Email;

            if (paypalDetails.PayPalReturnUserInfo.AddressInfo == null) return;

            if (paypalDetails.PayPalReturnUserInfo.AddressInfo.Street != null)
                ucUserDetails.Address1 = paypalDetails.PayPalReturnUserInfo.AddressInfo.Street;
                
            if (paypalDetails.PayPalReturnUserInfo.AddressInfo.Street2 != null)
                ucUserDetails.Address2 = paypalDetails.PayPalReturnUserInfo.AddressInfo.Street2;
                
            if (paypalDetails.PayPalReturnUserInfo.AddressInfo.City != null)
                ucUserDetails.Town = paypalDetails.PayPalReturnUserInfo.AddressInfo.City;

            if (paypalDetails.PayPalReturnUserInfo.AddressInfo.Postcode != null)
            {
                ucUserDetails.PostCode = paypalDetails.PayPalReturnUserInfo.AddressInfo.Postcode;
            }
                
            if (paypalDetails.PayPalReturnUserInfo.AddressInfo.State != null)
                ucUserDetails.PostCode = paypalDetails.PayPalReturnUserInfo.AddressInfo.State;

            if (paypalDetails.PayPalReturnUserInfo.AddressInfo.CountryCode != null)
                ucUserDetails.Country = paypalDetails.PayPalReturnUserInfo.AddressInfo.CountryCode;
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