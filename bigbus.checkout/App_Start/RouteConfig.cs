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
            routes.IgnoreRoute("Content/{*pathInfo}");

            routes.MapRoute(
               name: "CheckoutSuccess",
               url: "{controller}/{action}/{sid}",
               defaults: new { controller = "Default", action = "Index", sid = UrlParameter.Optional }
           );

           routes.MapRoute(
               name: "CheckoutWithMicrosite",
               url: "{micrositeid}/{controller}/{action}/{id}",
               defaults: new {micrositeid = "london", controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

