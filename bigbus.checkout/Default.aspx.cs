using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.Controllers;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Enums;
using Services.Infrastructure;
using Services.Implementation;

namespace bigbus.checkout
{
    public partial class Default : BasePage
    {
        protected int ItemIndex = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ConfigurationManager.AppSettings["Testing"] == null || !ConfigurationManager.AppSettings["Testing"].Equals("1"))
                {
                    Response.Redirect("~/bookingaddress.aspx");
                    return;
                }

                //txtCookieValue.Text = "0%253A2%253AzdqzxUdeGzV1wCalCSxHJRizo8AtSVKi%253ABwmx0AmImyc%252FLkkGRnfhpdhbq%252B3JpOCVhdWXyHlwS9U%253D"; // GetLatestIncreasedCookieValue();
                PlantCookie(sender, e);
                //LoadBasketLines();
            }
        }

        protected void LoadBasketLines()
        {
            rptItems.DataSource =MagentoTestController.TestBasketItems;
            rptItems.DataBind();
        }

        protected string GetLatestIncreasedCookieValue()
        {
            var basket = BasketService.GetLatestBasket();
            return basket != null ? IncreaseCookieValue(basket.ExternalCookieValue) : "001";
        }

        protected void PlantCookie(object sender, EventArgs e)
        {
            txtCookieValue.Text = GetLatestIncreasedCookieValue();
            //txtCookieValue.Text = "0%253A2%253AzdqzxUdeGzV1wCalCSxHJRizo8AtSVKi%253ABwmx0AmImyc%252FLkkGRnfhpdhbq%252B3JpOCVhdWXyHlwS9U%253D";
            AuthenticationService.SetCookie(ExternalBasketCookieName, SessionCookieDomain, txtCookieValue.Text);
        }

        private string IncreaseCookieValue(string cookieValue)
        {
            if (string.IsNullOrEmpty(cookieValue) || cookieValue.Length < 3)
                return string.Empty;

            var val = cookieValue.Substring(2);
            var newval = Convert.ToInt32(val);
            newval++;
            return cookieValue.Replace(val, newval.ToString());
        }

        protected void GoToCheckout(object sender, EventArgs e)
        {
            Response.Redirect("~/BookingAddress.aspx");
        }

        protected void PostUserToDetailsPage(object sender, EventArgs e)
        {
            //var cookieVal = GetLatestIncreasedCookieValue();
            //AuthenticationService.SetCookie(ExternalBasketCookieName, SessionCookieDomain, cookieVal);
            Response.Redirect("~/BookingAddress.aspx");
        }
        
    }
}