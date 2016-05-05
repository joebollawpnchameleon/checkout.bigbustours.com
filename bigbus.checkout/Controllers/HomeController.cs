using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace bigbus.checkout.Controllers
{
    public class HomeController : Controller
    {
       public ActionResult Index()
        {
            return View();
        }
    }
}