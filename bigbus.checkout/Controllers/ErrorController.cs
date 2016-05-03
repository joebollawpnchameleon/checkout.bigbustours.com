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
        
        public ActionResult BookingError()
        {
            return View("StandardErrorView");
        }

        public ActionResult BookingFailed()
        {
            return View("StandardErrorView");
        }

        #endregion

        #region LocalErrors

        public ActionResult BasketProcessingError()
        {
            return View("StandardErrorView");
        }

        public ActionResult OrderCreationError()
        {
            return View("StandardErrorView");
        }

        public ActionResult ExternalAPiError()
        {
            return View("StandardErrorView");
        }

        #endregion

    }
}