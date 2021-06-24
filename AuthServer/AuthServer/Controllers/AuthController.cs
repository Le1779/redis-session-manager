using AuthServer.Models;
using AuthServer.Models.Session;
using JWT.Exceptions;
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
        [Route("validate")]
        [HttpGet]
        public IHttpActionResult Validate(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Content(HttpStatusCode.Unauthorized, "Token Not Filled.");
                }

                token = HttpUtility.UrlDecode(token);
                string content = new JwtManager().Validate(token);
                return Content(HttpStatusCode.OK, content);
            }
            catch (InvalidTokenPartsException)
            {
                return Content(HttpStatusCode.Unauthorized, "InvaildToken.");
            }
            catch (TokenExpiredException)
            {
                return Content(HttpStatusCode.Unauthorized, "TokenExpired.");
            }
            catch (SignatureVerificationException)
            {
                return Content(HttpStatusCode.Unauthorized, "SignatureVerificationFailed.");
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
