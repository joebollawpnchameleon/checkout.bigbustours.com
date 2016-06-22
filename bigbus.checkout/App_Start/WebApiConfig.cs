using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace bigbus.checkout.App_Start
{
    public class WebApiConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapHttpRoute(
            //     name: "API Default",
            //     routeTemplate: "api/{controller}/{id}",
            //     defaults: new { id = RouteParameter.Optional }
            // );

            routes.MapHttpRoute("API Default", "api/{controller}/{action}/{id}",
            new { id = RouteParameter.Optional });
        }
    }
}