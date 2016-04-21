using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.ConstrainedExecution;
using Common.Enums;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Model;
using Services.Implementation;
using PCI = Common.Model.Pci;
using Services.Infrastructure;
using Environment = System.Environment;
using PCIServe = Services.Implementation.PciApiService;

namespace bigbus.checkout
{
    public partial class BookingAddress : BasePage
    {
        #region Injectable properties (need to be public)

            public IApiConnectorService ApiConnector { get; set;}
            public IBasketService BasketService { get; set; }
            public ICountryService CountryService { get; set; }
            public IUserService UserService { get; set; }
            public IPciApiServiceNoASync PciApiService { get;set; }
            public ICurrencyService CurrencyService { get; set; }
            public ITicketService TicketService { get; set; }
            public IAuthenticationService AuthenticationService { get; set; }
            public ISiteService SiteService { get; set; }
        #endregion
           

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTitles();
                LoadCountries();
                 GetExternalBasket();
                //make sure language is generated from base page.
            }
           
        }

        private void LoadTitles()
        {
            TitleList.DataSource = Enum.GetNames(typeof(Titles));
            TitleList.DataBind();
        }

        private void LoadCountries()
        {
            CountryList.DataSource = CountryService.GetAllCountriesWithNewOnTop("US,GB,AU,CA,FR,DE,HK,AE", "eng");
            CountryList.DataTextField = "Value";
            CountryList.DataValueField = "Key";
            CountryList.DataBind();
        }

        protected void GetExternalBasket()
        {
            //Get user basket cookie at this level.
            var externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);

            if (string.IsNullOrEmpty(externalSessionId))
            {
                //show message for empty session
                DisplayError(GetTranslation("Session_Details_NotFound"), "External/Magento session not found!");
                return;
            }

            //first check if we don't already have this basket saved
            if (BasketService.DoesBasketExist(externalSessionId)) return;

            //retrieve basket from Magento and persist it and create session.
            var basket = ApiConnector.GetExternalBasketByCookie(externalSessionId);
            //check basket has been retrieved
            if (basket == null)
            {
                DisplayError(GetTranslation("Session_Basket_NotFound"), "Basket retrieval failed with cookievalue: " + externalSessionId);
                return;
            }

            //this is required for persisting the basket.
            basket.ExternalCookieValue = externalSessionId;
            var basketGuid = BasketService.PersistBasket(basket);

            //check basket has been saved
            if (basketGuid == Guid.Empty)
            {
                DisplayError(GetTranslation("Session_Save_Failed"), "Basket Save failed with cookievalue: " + externalSessionId);
                return;
            }
            //create local session if everything OK.
            AuthenticationService.CreateNewSession(basketGuid, basket.CurrencyId, SessionCookieDomain,
                SessionCookieName);

            AuthenticationService.SetCookie(BasketCookieName, SessionCookieDomain, basketGuid.ToString());
        }

        private void DisplayError(string message, string logMessage)
        {
            ltError.Text = message;
            Log(logMessage);
            dvErrorSummary.Visible = true;
            dvAddressDetails.Visible = false;
        }
        
        protected void ContinueToCheckout(object sender, EventArgs e)
        {
            if (!Page.IsValid || !DoubleCheckTsAndCs())
            {
                return;
            }

            Log("Start Payment Process: " + DateTime.Now + Environment.NewLine + "Lock session for checkout: " + DateTime.Now);
           
            var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
            var customerSession = AuthenticationService.PutSessionInCheckoutMode(sessionId.ToString());

            var customer = new Customer
            {
                Title = TitleList.SelectedValue,
                Firstname = RegisterFirstNameTextbox.Text,
                Lastname = RegisterLastnameTextbox.Text,
                Email = RegisterEmailTextbox.Text,
                AddressLine1 = AddressLine1Textbox.Text,
                AddressLine2 = AddressLine2Textbox.Text,
                City = TownTextbox.Text,
                PostCode = PostCodeTextbox.Text,
                CountryId = CountryList.SelectedValue,
                StateProvince = StateTextbox.Text,
                LanguageId = CurrentLanguageId,
                CurrencyId = customerSession.CurrencyId,
                MicroSiteId = MicrositeId,
                Authorised = false,
                ReceiveNewsletter = false
            };

            UserService.CreateCustomer(customer);

            if (customer.Id == Guid.Empty)
            {
                DisplayError(GetTranslation("FailedToCreateUser"), "User creation failed.");
                return;
            }

            StartCheckOut(customer);
        }
        
        private void StartCheckOut(Customer customer)
        {
            var externalSessionId = AuthenticationService.GetExternalSessionId(ExternalBasketCookieName);
            var basket = BasketService.GetBasketBySessionId(externalSessionId);

            if (basket == null)//we are in trouble.
            {
                DisplayError(GetTranslation("NoBasketForExternalSessionId"), "No Basket was found matching sessionId: " + externalSessionId);
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
            
            var pciRequestSuccess = SendPciBasket(pciBasket);

            if (pciRequestSuccess)
            {
                RedirectToPciLandingPage();
            }
            else
            {
                var sessionId = AuthenticationService.GetSessionId(SessionCookieName);
                AuthenticationService.MoveSessionOutOfCheckoutMode(sessionId.ToString());
                Response.Redirect("BookingError.aspx");
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
                
            }

            return pciRequestSuccess;
        }
        
        public void RedirectToPciLandingPage()
        {
            var subSite = SubSite.Equals("international", StringComparison.OrdinalIgnoreCase) ? "london" : SubSite;
            Response.Redirect(string.Format(PciDomain, CurrentLanguageId, subSite)  + PciLandingPagePath);
        }

        public bool DoubleCheckTsAndCs()
        {
            if (AgreeTermsAndConditions.Checked) return true;

            TAndCValidator.ErrorMessage = GetTranslation("Pleasereadandagreetotandc");
            TAndCValidator.Text = "*" + GetTranslation("Required");
            TsAndCsLit.Text = "<span style=\"color:red\">" + GetTranslation("Pleasereadandagreetotandc") + "</span>";
            TsAndCsStarLit.Text = "<span style=\"color:red\">*</span>";
            return false;
        }
        
        //*** implement differently.
        protected void ValidateRegistration(object source, ServerValidateEventArgs value)
        {
            //if (ThisSession != null)
            //{
            //    if (ThisSession.PayPal_Token == null)
            //    {
                    string requiredText = GetTranslation("Required");

                    FirstnameValidator.ErrorMessage = string.Empty;
                    LastnameValidator.ErrorMessage = string.Empty;
                    EmailValidator.ErrorMessage = string.Empty;

                    FirstnameValidator.Text = string.Empty;
                    LastnameValidator.Text = string.Empty;
                    EmailValidator.Text = string.Empty;

                    TAndCValidator.ErrorMessage = string.Empty;
                    TAndCValidator.Text = string.Empty;

                    AddressValidator.ErrorMessage = string.Empty;
                    TownValidator.ErrorMessage = string.Empty;
                    PostcodeValidator.ErrorMessage = string.Empty;
                    CountryValidator.ErrorMessage = string.Empty;

                    AddressValidator.Text = string.Empty;
                    TownValidator.Text = string.Empty;
                    PostcodeValidator.Text = string.Empty;
                    CountryValidator.Text = string.Empty;

                    ValidationErrorSummary.HeaderText = GetTranslation("PleaseFixTheFollowingErrors");

                    //if (string.IsNullOrEmpty(customer.FirstName))
                    //{
                    //    FirstnameValidator.ErrorMessage = GetTranslation("Pleaseenteryourfirstname");
                    //    FirstnameValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}

                    //if (string.IsNullOrEmpty(customer.LastName))
                    //{
                    //    LastnameValidator.ErrorMessage = GetTranslation("Pleaseenteryourlastname");
                    //    LastnameValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}

                    //if (string.IsNullOrEmpty(customer.Email))
                    //{
                    //    EmailValidator.ErrorMessage = GetTranslation("Pleaseenteryouremailaddress");
                    //    EmailValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}

                    //if (!CheckValidEmail(customer.Email))
                    //{
                    //    EmailValidator.ErrorMessage = GetTranslation("Pleaseenteravalidemailaddress");
                    //    EmailValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}

                    //if (string.IsNullOrEmpty(customer.Address1))
                    //{
                    //    AddressValidator.ErrorMessage = GetTranslation("Pleaseenteryouraddress");
                    //    AddressValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}

                    //if (string.IsNullOrEmpty(customer.City))
                    //{
                    //    TownValidator.ErrorMessage = GetTranslation("Pleasenteryourtowncity");
                    //    TownValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}

                    //if (customer.Country == "-" || customer.Country == "--")
                    //{
                    //    CountryValidator.ErrorMessage = GetTranslation("Booking_SelectCountryError");
                    //    CountryValidator.Text = "*" + requiredText;
                    //    value.IsValid = false;
                    //}
                    //else if (customer.Country == "GB" || customer.Country == "US")
                    //{
                    //    if (string.IsNullOrEmpty(customer.PostCode))
                    //    {
                    //        PostcodeValidator.ErrorMessage = GetTranslation("Pleaseenteryourpostzipcode");
                    //        PostcodeValidator.Text = "*" + requiredText;
                    //        value.IsValid = false;
                    //    }
                    //}


                    if (!AgreeTermsAndConditions.Checked)
                    {
                        TAndCValidator.ErrorMessage = GetTranslation("Pleasereadandagreetotandc");
                        TAndCValidator.Text = "*" + requiredText;
                        value.IsValid = false;
                    }

            //    }

            //}
        }
    }
}