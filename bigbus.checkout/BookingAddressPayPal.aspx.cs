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

namespace bigbus.checkout
{
    public partial class BookingAddressPayPal : BasePage
    {
        public ICheckoutService CheckoutService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetPayerDetails();
        }

        protected void CompletePaypalCheckout(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var orderId = string.Empty;
            var session = GetSession();

            try
            {
                // end create new user
                var newUser = CreateUser();
               
                session.InCheckoutProccess = false;
                session.InOrderCreationProcess = true;

                AuthenticationService.UpdateSession(session);

                if (session.BasketId == null)
                {
                    Log("Invalid or missing basket in session. Id:" + session.Id);
                    return;
                }

                var basket = BasketService.GetBasket(session.BasketId.Value);
                
                var isoCurrencyCode = CurrencyService.GetCurrencyIsoCodeById(session.CurrencyId);

                Log("Paypal log- payment to take next, next is order");

                var payPalReturn = PaypalService.ConfirmPayment((basket.Total.ToString(CultureInfo.InvariantCulture)), isoCurrencyCode, session.PayPalToken, session.PayPalPayerId);

                Log("Paypal log- payment taken, next is order, paypal return status: " + payPalReturn.ErrorMessage);

                session.PayPalOrderId = payPalReturn.Transaction_Id;
                 
                AuthenticationService.UpdateSession(session);

                if (payPalReturn.IsError)
                {
                    throw new Exception(payPalReturn.ErrorMessage);
                }
                
                var order = CheckoutService.CreateOrderPayPal(session, basket, newUser, GetClientIpAddress(), CurrentLanguageId,
                    MicrositeId);

                CheckoutService.CreateAddressPaypal(order, session, newUser);
                
            }
            catch (Exception ex)
            {
                Log("Paypal Payment Error: " + ex.Message);
                Response.Redirect(@"~/Error/PayPalProcessingError/standard" );
            }
            finally
            {
                UnlockSessionFromOrderCreationLock(session);
                Log("PayPal Payment - Session unlocked");
            }

            Response.Redirect(@"~/Checkout/Completed/" + orderId);
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
                ReceiveNewsletter = false
            };

            var user = UserService.CreateCustomer(customer);

            if (customer.Id != Guid.Empty) return user;

            DisplayError(GetTranslation("FailedToCreateUser"), "User creation failed.");
            return null;
        }


        private Session GetSession()
        {
            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            return AuthenticationService.GetSession(sessionId);
        }

        private void GetPayerDetails()
        {
            var session = GetSession();

            var paypalDetails = PaypalService.ConfirmCheckoutDetails(session.PayPalToken);

            session.PayPalPayerId = paypalDetails.PayPalReturnUserInfo.Payer_Id;
            session.PayPalOrderId = paypalDetails.Transaction_Id;

            AuthenticationService.UpdateSession(session);

            if (paypalDetails.PayPalReturnUserInfo.Firstname != null)
            {
                PopulateCustomerDetails(paypalDetails);
                //populate user details form. and update address session details.
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


    }
}