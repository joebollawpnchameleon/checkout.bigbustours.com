using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Model;
using Environment = System.Environment;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace bigbus.checkout
{
    public partial class BookingAddress : BasePage
    {
        protected string TotalSummary { get; set; }
        public override string DefaultPageTitle { get { return string.Concat("Big Bus Tours - ", CurrentSite.Name, " - AddressDetails"); } }
        public override string CmsPageType { get { return "BookingAddress"; } }
        public override string CmsPageIdentifier { get { return string.Concat("BookingAddress_", MicrositeId, "_", CurrentLanguageId); } }
        protected override bool AutoLoadChameleonMetaTags { get { return true; } }       
        public override bool InjectViewportMetaTag { get { return true; } }

        #region PageEvents

        protected void Page_Load(object sender, EventArgs e)
        {
            //***  dvErrorSummary.Visible = IsPostBack && !IsValid   -- implement#

            if (IsPostBack) return;

            //make sure basket cookie is available otherwise show only error to user
            if(string.IsNullOrEmpty(ExternalSessionCookieValue))
            { 
                //show message for empty session
                DisplayError(GetTranslation("Session_Details_NotFound"), "External/Magento session not found!");
                return;
            }

            InitControls();
            LoadBasket();
        }

       
        protected void ContinueShopping(object sender, EventArgs e)
        {
            Response.Redirect(BornBaseUrl);
        }

        #endregion

        #region UiLoadFunctions

        private void InitControls()
        {
            ltError.Text = string.Empty;
            dvErrorSummary.Visible = false;
            pPaypal.Visible = true;
            ucUserDetails.Visible = true;
            btnContinueCheckout.Visible = true;
        }

        protected void LoadBasket()
        {
            Log("BookingAddress => LoadBasket().External cookie picked up BORN SessionId: " + ExternalSessionCookieValue + " Going to retrieve basket from DB.");

            //first check if we don't already have this basket saved
            var dbBasket = BasketService.GetBasketBySessionId(ExternalSessionCookieValue);

            if (dbBasket != null)// if basket is there, update it simply.
            {
                Log("Basket found. Deleting sessionid:" + ExternalSessionCookieValue);
                BasketService.DeleteBasket(dbBasket);
            }

            //retrieve basket from Magento and persist it and create session.
            var basket = ApiConnector.GetExternalBasketByCookie(Server.UrlEncode(ExternalSessionCookieValue));

            //check basket has been retrieved
            if (basket == null)
            {
                DisplayError(GetTranslation("Session_Basket_NotFound"), "BookingAddress => LoadBasket().Basket retrieval failed with cookievalue: " + ExternalSessionCookieValue);
                return;
            }

            //this is required for persisting the basket.
            basket.ExternalCookieValue = ExternalSessionCookieValue;

            //make sure basket is valid
            if (!BasketService.IsBornBasketValid(basket))
            {
                DisplayError(GetTranslation("Session_Basket_NotFound"), "BookingAddress => LoadBasket().Invalid BORN Basket: " + ExternalSessionCookieValue);
                return;
            }

            Log("BookingAddress => LoadBasket().Basket retrieved successfully from BORN. External SessionId: " + ExternalSessionCookieValue);
            
            var basketGuid = BasketService.PersistBasket(basket);

            DisplayBasketDetails(basket);

            //check basket has been saved
            if (basketGuid == Guid.Empty)
            {
                DisplayError(GetTranslation("Session_Save_Failed"), "BookingAddress => LoadBasket().Basket Save failed with cookievalue: " + ExternalSessionCookieValue);
                return;
            }

            Log("BookingAddress => LoadBasket().Basket saved to Database Successfully. basketid: " + basketGuid);

            //create local session if everything OK.
            AuthenticationService.LinkSessionToBasketAndCurrency(CurrentSession, basketGuid, basket.CurrencyId);

            Log("BookingAddress => LoadBasket().Session Created Id: " + CurrentSession.Id);

            AuthenticationService.SetCookie(BasketCookieName, SessionCookieDomain, basketGuid.ToString());

            Log("BookingAddress => LoadBasket().Basket details loaded from API and persisted to DB successfully.");
        }
        
        private void DisplayBasketDetails(BornBasket basket)
        {
            var currency = CurrencyService.GetCurrencyByCode(basket.CurrencyCode);
            CurrentLanguageId = basket.Language;
            MicrositeId = basket.LastMicroSite;
            TotalSummary = currency.Symbol + basket.Total.ToString("F");

            LoadMasterValues();

            ucBasketDisplay.AddMoreUrl = ConfigurationManager.AppSettings["BornAddMoreTicketUrl"];
            ucBasketDisplay.ParentPage = this;
            ucBasketDisplay.ShowActionRow = true;
            ucBasketDisplay.TotalString = TotalSummary;

            var itemList = basket.BasketItems.Select(item => new BasketDisplayVm
            {
                TicketName = item.ProductName + " - " + item.Microsite,
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

            var pciRequestSuccess = PciApiService.SendPciBasket(pciBasket, CurrentLanguageId, MicrositeId);

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
              
        public void RedirectToPciLandingPage()
        {
            var subSite = SubSite.Equals("international", StringComparison.OrdinalIgnoreCase) ? "london" : SubSite;
            Response.Redirect(string.Format(PciDomain, subSite)  + PciLandingPagePath);
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
           // var session = AuthenticationService.GetSession(sessionId);
            PaypalService.SetUserSessionId(sessionId.ToString());
                     
            var paypalOrder = BasketService.BuildPayPalOrder(basket);
            var paypalResult = PaypalService.ShortcutExpressCheckout(PayPalSuccessUrl, PayPalCancelUrl, null, paypalOrder, false, "Mark");

            if (paypalResult.IsError)
            {
                Response.Redirect("~/Error/ExternalAPiError/");
            }
            else
            {
                CurrentSession.PayPalToken = paypalResult.Token;
                CurrentSession.PayPalOrderId = paypalResult.Transaction_Id;
                AuthenticationService.UpdateSession(CurrentSession);
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
            pPaypal.Visible = false;
            ucUserDetails.Visible = false;
            btnContinueCheckout.Visible = false;
        }

        #endregion

    }
}