using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bigbus.checkout.Models;
using Services.Infrastructure;

namespace bigbus.checkout.Controllers
{
    public class CheckoutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: Checkout
        public ActionResult Success()
        {
            Response.Redirect("~/BookingSuccess.aspx");
            return null;
        }

        public ActionResult Completed(string sid)
        {
            var model = new OrderConfirmationVm {OrderId = sid};

            return View(model);
        }

        public ActionResult Cancelled()
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

        public ActionResult Failure()
        {
            return View();
        }
    }
}