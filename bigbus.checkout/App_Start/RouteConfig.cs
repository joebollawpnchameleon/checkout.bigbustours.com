using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace bigbus.checkout
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "CheckoutSuccess",
               url: "{controller}/{action}/{sid}",
               defaults: new { controller = "Home", action = "Index", sid = UrlParameter.Optional }
           );
          
            routes.MapRoute(
               name: "Checkout",
               url: "{microSiteID}/{controller}/{action}",
               defaults: new { microSiteID = "london", controller = "Default", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

