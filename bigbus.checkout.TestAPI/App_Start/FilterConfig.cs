﻿using System.Web;
using System.Web.Mvc;

namespace bigbus.checkout.TestAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
