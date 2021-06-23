using AuthServer.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthServer.Controllers
{
    [RoutePrefix("api/auth")]
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
                UserSession.Password = password;

                return Content(HttpStatusCode.OK, "OK");
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
