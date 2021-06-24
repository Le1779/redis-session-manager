using AuthServer.Models;
using AuthServer.Models.Session;
using AuthServer.Models.ViewModel;
using System;
namespace AuthServer.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Login(string returnUrl = "")
        {
            if (UserSession.Token != null && !string.IsNullOrEmpty(returnUrl)) 
            {
                returnUrl = returnUrl + "?token=" + UserSession.Token;
                return Redirect(returnUrl);
            }

            LoginViewModel viewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //驗證帳號
            string token = new JwtManager().Generate(UserSession.SessionID);
            model.ReturnUrl = model.ReturnUrl + "?token=" + token;
            UserSession.Account = model.Account;
            UserSession.Token = token;
            return Redirect(model.ReturnUrl);
        }
    }
}
