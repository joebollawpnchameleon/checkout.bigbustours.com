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
        public IAuthenticationService AuthenticationService { get; set; }
        public IImageDbService ImageDbService { get; set; }

        public IBasketService BasketService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var basket = BasketService.GetBasket(new Guid("B2E65DBA-925E-4407-9F1E-A004314A7021"));
            var lines = basket.BasketLines;
        }

        protected void PlantCookie(object sender, EventArgs e)
        {
            AuthenticationService.SetCookie(ExternalBasketCookieName, SessionCookieDomain, txtCookieValue.Text);

            var imageFolder = ImageDbService.GetImageFolder("london");

            var name = imageFolder.FolderName;

            //var cookie = new HttpCookie(ExternalBasketCookieName,txtCookieValue.Text)
            //{
            //    Domain = SessionCookieDomain,
            //    HttpOnly = true
            //};

            //HttpContext.Current.Response.Cookies.Add(cookie);
        }

        protected void GoToCheckout(object sender, EventArgs e)
        {
            Response.Redirect("~/BookingAddress.aspx");
        }
    }
}