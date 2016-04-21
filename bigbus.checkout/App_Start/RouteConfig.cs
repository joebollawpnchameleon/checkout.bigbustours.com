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
               name: "Checkout",
               url: "{microSiteID}/{controller}/{action}",
               defaults: new { microSiteID = "london", controller = "Default", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{microSiteID}/{controller}/{action}/{id}",
                defaults: new { microSiteID = "london", controller = "Checkout", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

