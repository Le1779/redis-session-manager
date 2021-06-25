using AuthServer.Models;
using JWT.Exceptions;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Net;
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
                string json = new JwtManager().Validate(token);
                dynamic data = JObject.Parse(json);
                string key = "{/_" + data.data + "}_Data";

                var conn = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                var db = conn.GetDatabase();
                bool isExists = db.KeyExists(key);
                var sessionToken = db.HashGet(key, "Token");

                if (isExists && !sessionToken.IsNull)
                {
                    db.KeyExpire(key, new TimeSpan(0, 0, 20, 0));
                    return Content(HttpStatusCode.OK, "OK");
                }
                else 
                {
                    return Content(HttpStatusCode.Unauthorized, "Session Expired.");
                }
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

        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Content(HttpStatusCode.BadRequest, "Token Not Filled.");
                }

                token = HttpUtility.UrlDecode(token);
                string json = new JwtManager().Validate(token);
                dynamic data = JObject.Parse(json);
                string key = "{/_" + data.data + "}_Data";
                var conn = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                var db = conn.GetDatabase();
                bool isExists = db.KeyExists(key);
                if (isExists) 
                {
                    db.KeyDelete(key);
                }
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
