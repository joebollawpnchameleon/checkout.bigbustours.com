using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Infrastructure;
using bigbus.checkout.ViewModels;

namespace bigbus.checkout.Controllers
{
    public class CheckoutController : Controller
    {
        public IApiConnectorService ApiConnector { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult UserDetails()
        {
            var test = ApiConnector;
            return View();
        }

        [HttpPost]
        public ActionResult CheckoutWithCreditCard(UserDetailVM userDetails)
        {
            if (!ModelState.IsValid)
            {
                return View("UserDetails", userDetails);
            }
            //do processing here.

            return Redirect("pcisite");
        }

        // GET: Checkout
        public ActionResult Success()
        {
            Response.Redirect("~/BookingSuccess.aspx");
            return null;
        }

        public ActionResult Completed(string id)
        {
            return View();
        }

        public ActionResult CancelBookingPayPal()
        {
            return View();
        }

        public ActionResult BookingSuccessPaypal()
        {
            var token = Request.QueryString["token"];
            var payerId = Request.QueryString["PayerID"];

            Response.Redirect(string.Format("~/BookingAddressPayPal.aspx?pptoken={0}&pppayerid={1}", token, payerId));
            return null;
        }
    }
}