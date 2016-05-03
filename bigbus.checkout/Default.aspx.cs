using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.Models;
using Services.Infrastructure;

namespace bigbus.checkout
{
    public partial class Default : BasePage
    {
        public IImageDbService ImageDbService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
        //    var basket = BasketService.GetBasket(new Guid("B2E65DBA-925E-4407-9F1E-A004314A7021"));
        //    var lines = basket.BasketLines;
            if(!IsPostBack)
                PlantCookie(sender, e);
        }

        protected void PlantCookie(object sender, EventArgs e)
        {

            //var imageFolder = ImageDbService.GetImageFolder("london");
            //var name = imageFolder.FolderName;

            var basket = BasketService.GetLatestBasket();
            txtCookieValue.Text = IncreaseCookieValue(basket.ExternalCookieValue);

            AuthenticationService.SetCookie(ExternalBasketCookieName, SessionCookieDomain, txtCookieValue.Text);

            //var cookie = new HttpCookie(ExternalBasketCookieName,txtCookieValue.Text)
            //{
            //    Domain = SessionCookieDomain,
            //    HttpOnly = true
            //};

            //HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private string IncreaseCookieValue(string cookieValue)
        {
            if (string.IsNullOrEmpty(cookieValue) || cookieValue.Length < 11)
                return string.Empty;

            var val = cookieValue.Substring(19);
            var newval = Convert.ToInt32(val);
            newval++;
            return cookieValue.Replace(val, newval.ToString());
        }

        protected void GoToCheckout(object sender, EventArgs e)
        {
            Response.Redirect("~/BookingAddress.aspx");
        }
    }
}