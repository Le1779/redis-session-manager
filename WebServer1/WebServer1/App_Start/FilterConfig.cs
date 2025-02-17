﻿using System.Web.Mvc;
using WebServer1.Filter;

namespace WebServer1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthenticationFilter());
        }
    }
}
