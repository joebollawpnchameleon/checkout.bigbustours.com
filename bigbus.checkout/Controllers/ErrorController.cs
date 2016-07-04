using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bigbus.checkout.Controllers
{
    public class ErrorController : Controller
    {
        #region PCIErrors

        public ActionResult PaymentError()
        {
            return View("StandardErrorView");
        }

        public ActionResult PaymentFailure()
        {
            return View("StandardErrorView");
        }

        #endregion

        #region LocalErrors

        public ActionResult PayPalProcessingError()
        {
            return View("StandardErrorView");    
        }

        public ActionResult BasketProcessingError()
        {
            return View("StandardErrorView");
        }

        public ActionResult OrderCreationError()
        {
            return View("StandardErrorView");
        }

        public ActionResult ExternalAPiError(string sid)
        {
            return View("StandardErrorView");
        }

        public ActionResult BookingError(string sid)
        {
            return View("StandardErrorView");
        }

        public ActionResult EvoucherError()
        {
            return View("StandardErrorView");
        }

        #endregion

    }
}