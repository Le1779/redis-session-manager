using System.Web;
using System.Web.Mvc;
using WebServer2.Filter;

namespace WebServer2
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
