using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Common.Enums;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Model;
using Common.Model.PayPal;
using Services.Implementation;
using PCI = Common.Model.Pci;
using Services.Infrastructure;
using Environment = System.Environment;
using PCIServe = Services.Implementation.PciApiService;

namespace bigbus.checkout
{
    public partial class BookingAddress : BasePage
    {
        protected string TotalSummary { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            InitControls();
            LoadBasket();
        }

        protected void LoadBasket()
        {
            //Get user basket cookie at this level.
            var externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
            
            if (string.IsNullOrEmpty(externalSessionId))
            {
                //show message for empty session
                DisplayError(GetTranslation("Session_Details_NotFound"), "External/Magento session not found!");
                return;
            }

            Log("BookingAddress => LoadBasket().External cookie picked up BORN SessionId: " + externalSessionId + " Going to retrieve basket from DB.");

            //first check if we don't already have this basket saved
            var dbBasket = BasketService.GetBasketBySessionId(externalSessionId);

            if (dbBasket != null)
            {
                Log("BookingAddress => LoadBasket().Basket Found Basket Id: " + dbBasket.Id);
                var currency = CurrencyService.GetCurrencyById(dbBasket.CurrencyId.ToString());
                TotalSummary = currency.Symbol + dbBasket.Total;

                Log("BookingAddress => LoadBasket().Displaying Basket Id: " + dbBasket.Id);
                DisplayBasketDetails(dbBasket, ucBasketDisplay, currency.Symbol);
                return;
            }

            Log("BookingAddress => LoadBasket().New basket needs retrieval from API. ExternalSessionId: " + externalSessionId);

            //retrieve basket from Magento and persist it and create session.
            var basket = ApiConnector.GetExternalBasketByCookie(externalSessionId);

            //check basket has been retrieved
            if (basket == null)
            {
                DisplayError(GetTranslation("Session_Basket_NotFound"), "BookingAddress => LoadBasket().Basket retrieval failed with cookievalue: " + externalSessionId);
                return;
            }

            Log("BookingAddress => LoadBasket().Basket retrieved successfully from BORN. External SessionId: " + externalSessionId);

            //this is required for persisting the basket.
            basket.ExternalCookieValue = externalSessionId;
            var basketGuid = BasketService.PersistBasket(basket);
          
            DisplayBasketDetails(basket);

            //check basket has been saved
            if (basketGuid == Guid.Empty)
            {
                DisplayError(GetTranslation("Session_Save_Failed"), "BookingAddress => LoadBasket().Basket Save failed with cookievalue: " + externalSessionId);
                return;
            }

            Log("BookingAddress => LoadBasket().Basket saved to Database Successfully. basketid: " + basketGuid);

            //create local session if everything OK.
            var ncSessionId = AuthenticationService.CreateNewSession(basketGuid, basket.CurrencyId, SessionCookieDomain,
                SessionCookieName);

            Log("BookingAddress => LoadBasket().Session Created Id: " + ncSessionId);

            AuthenticationService.SetCookie(BasketCookieName, SessionCookieDomain, basketGuid.ToString());

            Log("BookingAddress => LoadBasket().Basket details loaded from API and persisted to DB successfully.");
        }
        

        protected void ContinueShopping(object sender, EventArgs e)
        {
            //***change to Born home page.
            Response.Redirect("~/Default.aspx");
        }

        #region UiLoadFunctions

        
        private void InitControls()
        {
            ltError.Text = string.Empty;
            dvErrorSummary.Visible = false;
            //btnCancel.Text = GetTranslation("Back");
            ucUserDetails.Visible = true;
            btnContinueCheckout.Visible = true;
        }

        private void DisplayBasketDetails(BornBasket basket)
        {
            var currency = CurrencyService.GetCurrencyByCode(basket.CurrencyCode);
            TotalSummary = currency.Symbol + basket.Total;

            ucBasketDisplay.AddMoreUrl = ConfigurationManager.AppSettings["BornAddMoreTicketUrl"];
            ucBasketDisplay.ParentPage = this;
            ucBasketDisplay.ShowActionRow = true;
            ucBasketDisplay.TotalString = TotalSummary;

            var itemList = basket.BasketItems.Select(item => new BasketDisplayVm
            {
                TicketName = item.ProductName,
                Date = GetTranslation("OpenDayTicket"), 
                Quantity = item.Quantity, 
                Title = item.TicketType.ToString(),
                TotalSummary = currency.Symbol + item.Total
            }).ToList();

            ucBasketDisplay.DataSource = itemList;
        }

        

        #endregion

        #region commonfunctions

        private CustomerSession UpdateCustomerSession()
        {
            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            return AuthenticationService.PutSessionInCheckoutMode(sessionId.ToString());
        }

        private Basket GetBasketByExternalSession()
        {
            var externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
            return BasketService.GetBasketBySessionId(externalSessionId);
        }
        #endregion

        #region CreditCardPayment

        protected void CheckoutWithCreditCard(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                LoadBasket();
                return;
            }

            Log("Start Payment Process: " + DateTime.Now + Environment.NewLine + "Lock session for checkout: " + DateTime.Now);

            var customerSession = UpdateCustomerSession();

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
                CurrencyId = customerSession.CurrencyId,
                MicroSiteId = MicrositeId,
                Authorised = false,
                ReceiveNewsletter = ucUserDetails.Subscribed,
                ExpectedTravelDate =  ucUserDetails.ExpectedTravelDate
            };

            UserService.CreateCustomer(customer);

            if (customer.Id == Guid.Empty)
            {
                DisplayError(GetTranslation("FailedToCreateUser"), "User creation failed.");
                return;
            }
            Log("User created starting checkout. USerId:" + customer.Id);

            StartCheckOut(customer);
        }
        
        private void StartCheckOut(Customer customer)
        {
            Log("Starting checkout for customer id: " + customer.Id);

            var basket = GetBasketByExternalSession();

            if (basket == null)//we are in trouble.
            {
                var externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
                DisplayError(GetTranslation("Session_Basket_NotFound"), "No Basket was found matching sessionId: " + externalSessionId);
                return;
            }

            //connect this customer to the basket
            var userConnected = BasketService.ConnectUserToBasket(customer.Id, basket.Id);

            if (!userConnected)
            {
                Log("User failed to connect to basket id " + basket.Id);
            }

            //build PCI Basket
            var pciBasket = BasketService.GetPciBasket(customer, basket);
            pciBasket.IPAddress = GetClientIpAddress();

            Log("Sending Basket to PCI BasketId: " + basket.Id);

            var pciRequestSuccess = SendPciBasket(pciBasket);

            if (pciRequestSuccess)
            {
                Log("Basket sent success to PCI BasketId: " + basket.Id + "redirecting to PCI landing");
                RedirectToPciLandingPage();
            }
            else
            {
                Log("PCI interaction failed. basketid: " + basket.Id);
                var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
                AuthenticationService.MoveSessionOutOfCheckoutMode(sessionId.ToString());
                Response.Redirect(@"~/Error/BookingError/");
            }
        }

        public bool SendPciBasket(PCI.Basket pciBasket)
        {
            var pciRequestSuccess = true;
            var pciRequestResponse = string.Empty;
           
            try
            {
                var result = PciApiService.SendPostRequest(CurrentLanguageId, SubSite, pciBasket);
             
                pciRequestResponse = result;
            }
            catch (Exception exception)
            {
                Log("PCI web request error: " + DateTime.Now + " - Exception.Message: " + exception.Message);

                if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.Message))
                {
                    Log("PCI web request error: " + DateTime.Now + " - InnerException.Message: " + exception.InnerException.Message);
                }
                pciRequestSuccess = false;
            }

            if (pciRequestSuccess && !pciRequestResponse.Equals("\"" + pciBasket.ID + "\"", StringComparison.OrdinalIgnoreCase))
            {
                Log("PCI web request error: " + DateTime.Now + " - Returned basket id doesn't match: " + pciRequestResponse);
                pciRequestSuccess = false;
            }
            else if (pciRequestSuccess &&
                     pciRequestResponse.Equals("\"" + pciBasket.ID + "\"", StringComparison.OrdinalIgnoreCase))
            {//set basket cookie
                Log("Pci Send Basket successful!");
            }

            return pciRequestSuccess;
        }
        
        public void RedirectToPciLandingPage()
        {
            var subSite = SubSite.Equals("international", StringComparison.OrdinalIgnoreCase) ? "london" : SubSite;
            Response.Redirect(string.Format(PciDomain, CurrentLanguageId, subSite)  + PciLandingPagePath);
        }

        #endregion

        #region paypalpayment

        protected void CheckoutWithPaypal(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                LoadBasket();
                return;
            }

            Log("Start Payment Process - Paypal: " + DateTime.Now + Environment.NewLine + "Lock session for checkout: " + DateTime.Now);

            var customerSession = UpdateCustomerSession();

            var basket = GetBasketByExternalSession();

            if (basket == null)//we are in trouble.
            {
                var externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
                DisplayError(GetTranslation("Session_Basket_NotFound"), "No Basket was found matching sessionId: " + externalSessionId);
                return;
            }
            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            var session = AuthenticationService.GetSession(sessionId);
            PaypalService.SetUserSessionId(sessionId.ToString());

            var cancelUrl = ConfigurationManager.AppSettings["PayPal.CancelURL"];
            var successUrl = ConfigurationManager.AppSettings["PayPal.SuccessURL"];

            var paypalOrder = BasketService.BuildPayPalOrder(basket);

            var paypalResult = PaypalService.ShortcutExpressCheckout(successUrl, cancelUrl, null, paypalOrder, false, "Mark");

            if (paypalResult.IsError)
            {
                Response.Redirect("~/Error/ExternalAPiError/");
            }
            else
            {
                session.PayPalToken = paypalResult.Token;
                session.PayPalOrderId = paypalResult.Transaction_Id;

                AuthenticationService.UpdateSession(session);
                Response.Redirect(paypalResult.RedirectURL);
            }
        }

        #endregion

        #region validation

        private void DisplayError(string message, string logMessage)
        {
            ltError.Text = message;
            Log(logMessage);
            dvErrorSummary.Visible = true;
            ucUserDetails.Visible = false;
            btnContinueCheckout.Visible = false;
        }

        #endregion

    }
}