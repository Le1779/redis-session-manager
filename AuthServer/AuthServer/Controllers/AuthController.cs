using AuthServer.Models;
using AuthServer.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AuthServer.Controllers
{
    [RoutePrefix("api/auth")]
    [EnableCors("*", "*", "*")]
    public class AuthController : ApiController
    {
        [Route("login")]
        [HttpGet]
        public IHttpActionResult Login(string account, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(account))
                {
                    return Content(HttpStatusCode.OK, "Account Not Filled.");
                }

                if (string.IsNullOrEmpty(password))
                {
                    return Content(HttpStatusCode.OK, "Password Not Filled.");
                }

                UserSession.Account = account;
                string token = new JwtManager().Generate(UserSession.SessionID);

                return Content(HttpStatusCode.OK, HttpContext.Current.Session.SessionID);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, "OK");
        }
    }
}
