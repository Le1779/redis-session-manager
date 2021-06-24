namespace WebServer1.Filter
{
    using System;
    using System.Net.Http;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;

    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            
            if (request.Cookies["Token"] != null)
            {
                string token = request.Cookies["Token"]["Value"];
                using (var client = new HttpClient()) 
                {
                    var response = client.GetAsync("https://localhost:44331/api/auth/validate?token=" + HttpUtility.UrlEncode(token));
                    response.Wait();
                    var result = response.Result;
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        HttpCookie tokenCookie = new HttpCookie("Token");
                        tokenCookie.Expires = DateTime.Now.AddDays(-1d);
                        filterContext.HttpContext.Response.AppendCookie(tokenCookie);
                        string redirectUrl = "https://localhost:44331/?returnUrl=http://localhost:58549/";
                        filterContext.Result = new RedirectResult(redirectUrl, true);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(request.QueryString["Token"]))
            {
                string token = request.QueryString["Token"];
                HttpCookie tokenCookie = new HttpCookie("Token");
                tokenCookie.Values.Add("Value", token);
                tokenCookie.Path = "/";
                filterContext.HttpContext.Response.AppendCookie(tokenCookie);
            }
            else 
            {
                string redirectUrl = "https://localhost:44331/?returnUrl=http://localhost:58549/";
                filterContext.Result = new RedirectResult(redirectUrl, true);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) {}
    }
}