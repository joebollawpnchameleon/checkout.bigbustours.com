using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.Models;
using Common.Enums;
using Services.Infrastructure;

namespace bigbus.checkout.Controls
{
    public partial class WebUserControl1 : UserControl
    {
        protected BasePage ParentPage
        {
            get { return (BasePage) this.Page; }    
        }

        public ICountryService CountryService { get; set; }


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
        public bool Subscribed { get { return ckSubscribe.Checked; }}
        public string ExpectedTravelDate { get { return txtExpectedTourDate.Text; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTitles();
                LoadCountries();
                InitControls();
            }
            
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

        private void InitControls()
        {
            ValidationErrorSummary.HeaderText = ParentPage.GetTranslation("PleaseFixTheFollowingErrors");

            rqVFirstName.Text = rqVFirstLastName.Text = rqVFirstEmail.Text = rqVAddress1.Text =
                rqVTown.Text = rqVPostCode.Text = rqVAddress1.Text = rqVTown.Text = rqVPostCode.Text =
                cstVCountry.Text = ParentPage.GetTranslation("Required");
            cstVTerms.Text = "*";

            rqVFirstName.ErrorMessage = ParentPage.GetTranslation("Pleaseenteryourfirstname");
            rqVFirstLastName.ErrorMessage = ParentPage.GetTranslation("Pleaseenteryourlastname");
            rqVFirstEmail.ErrorMessage = ParentPage.GetTranslation("Pleaseenteryouremailaddress");
            regexEmailValid.ErrorMessage = ParentPage.GetTranslation("Pleaseenteryouremailaddress");
            rqVAddress1.ErrorMessage = ParentPage.GetTranslation("Pleaseenteryouraddress");
            rqVTown.ErrorMessage = ParentPage.GetTranslation("Pleasenteryourtowncity");
            rqVPostCode.ErrorMessage = ParentPage.GetTranslation("Pleaseenteryourpostzipcode");
            cstVCountry.ErrorMessage = ParentPage.GetTranslation("Booking_SelectCountryError");
            cstVTerms.ErrorMessage = ParentPage.GetTranslation("Pleasereadandagreetotandc");
        }

        public void ValidateTermsAndConditions(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ckTermsAndConditions.Checked;
        }

        protected void ValidateCountry(object source, ServerValidateEventArgs args)
        {
            var selCountry = ddlCountryList.SelectedValue;
            
            //validate country
            args.IsValid = (!string.IsNullOrEmpty(selCountry) && !selCountry.Trim().Equals("-") &&
                            !selCountry.Trim().Equals("--"));
        }
    }
}