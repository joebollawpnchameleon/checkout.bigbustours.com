using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.Models;
using Common.Enums;
using Services.Implementation;
using Services.Infrastructure;

namespace bigbus.checkout.Controls
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        protected BasePage ParentPage
        {
            get { return (BasePage) this.Page; }    
        }

        public ICountryService CountryService { get; set; }

        public bool IsValid
        {
            get { return IsFormValid(); }
        }

        public string UserTitle
        {
            get { return TitleList.SelectedValue; }
        }

        public string FirstName { get { return txtFirstName.Text; } set { txtFirstName.Text = value; } }
        public string LastName { get { return txtLastName.Text; } set{txtLastName.Text=value;}}
        public string Email {get{ return txtEmail.Text; } set{txtEmail.Text=value;} }
        public string Address1 { get { return txtAddress1.Text; } set{txtAddress1.Text=value;} }
        public string Address2 {get{ return  txtAddress2.Text; }  set{txtAddress2.Text=value;}}
        public string Town  {get{ return  txtTown.Text; }  set{txtTown.Text=value;}}
        public string PostCode {get{ return  txtPostCode.Text; }  set{txtPostCode.Text=value;}}
        public string State {get{ return   txtState.Text; }  set{txtState.Text=value;}}
        public string Country {get{ return   ddlCountryList.SelectedValue; }  set{ddlCountryList.SelectedValue=value;}}
        public bool TermsAndConditions {get{ return  ckTermsAndConditions.Checked; } }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTitles();
            LoadCountries();
        }

        private void LoadTitles()
        {
            TitleList.DataSource = Enum.GetNames(typeof(Titles));
            TitleList.DataBind();
        }

        private void LoadCountries()
        {
            ddlCountryList.DataSource = CountryService.GetAllCountriesWithNewOnTop("US,GB,AU,CA,FR,DE,HK,AE", "eng");
            ddlCountryList.DataTextField = "Value";
            ddlCountryList.DataValueField = "Key";
            ddlCountryList.DataBind();
        }

        protected bool IsFormValid()
        {
            return true;
        }

        protected void ValidateRegistration(object source, ServerValidateEventArgs value)
        {
            //if (ThisSession != null)
            //{
            //    if (ThisSession.PayPal_Token == null)
            //    {
            //string requiredText = GetTranslation("Required");

            //FirstnameValidator.ErrorMessage = string.Empty;
            //LastnameValidator.ErrorMessage = string.Empty;
            //EmailValidator.ErrorMessage = string.Empty;

            //FirstnameValidator.Text = string.Empty;
            //LastnameValidator.Text = string.Empty;
            //EmailValidator.Text = string.Empty;

            //TAndCValidator.ErrorMessage = string.Empty;
            //TAndCValidator.Text = string.Empty;

            //AddressValidator.ErrorMessage = string.Empty;
            //TownValidator.ErrorMessage = string.Empty;
            //PostcodeValidator.ErrorMessage = string.Empty;
            //CountryValidator.ErrorMessage = string.Empty;

            //AddressValidator.Text = string.Empty;
            //TownValidator.Text = string.Empty;
            //PostcodeValidator.Text = string.Empty;
            //CountryValidator.Text = string.Empty;

            //ValidationErrorSummary.HeaderText = GetTranslation("PleaseFixTheFollowingErrors");

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


            //if (!AgreeTermsAndConditions.Checked)
            //{
            //    TAndCValidator.ErrorMessage = GetTranslation("Pleasereadandagreetotandc");
            //    TAndCValidator.Text = "*" + requiredText;
            //    value.IsValid = false;
            //}

            //    }

            //}
        }

        public bool DoubleCheckTsAndCs()
        {
            if (ckTermsAndConditions.Checked) return true;

            TAndCValidator.ErrorMessage = ParentPage.GetTranslation("Pleasereadandagreetotandc");
            TAndCValidator.Text = "*" + ParentPage.GetTranslation("Required");
            TsAndCsLit.Text = "<span style=\"color:red\">" + ParentPage.GetTranslation("Pleasereadandagreetotandc") + "</span>";
            TsAndCsStarLit.Text = "<span style=\"color:red\">*</span>";
            return false;
        }
    }
}