// <copyright file="DynacwAuthenticationFilter.cs" company="DynaComware Corp.">
// Copyrightc 2016, DynaComware Corp.
//
// All rights reserved. Syscom source code is an unpublished work and the
// use of a copyright notice does not imply otherwise. This source code contains
// confidential, trade secret material of Syscom. Any attempt or participation
// in deciphering, decoding, reverse engineering or in any way altering the source
// code is strictly prohibited, unless the prior written consent of Company Name
// is obtained.
// </copyright>
// <date>       Created : 2016/7/18   </date>
// <brief>      Description :
//              負責驗證使用者
// </brief>
// <author>     Henry
// </author>

namespace DynaFlowWeb.Web.Filter
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;

    /// <summary>
    /// 記錄使用者動作的類別
    /// </summary>
    public class DynacwAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        /// <summary>
        /// 是否經過授權，若沒有經過授權的話則會被導到Login頁面
        /// </summary>
        /// <param name="filterContext">filterContext</param>
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            string token = filterContext.HttpContext.Request.QueryString["Token"];
            if (string.IsNullOrEmpty(token))
            {
                token = (string)HttpContext.Current.Session["Token"];

                if (string.IsNullOrEmpty(token))
                {
                    var url = filterContext.HttpContext.Request.Url.OriginalString;
                    string redirectUrl = "https://localhost:44331/?returnUrl=" + HttpUtility.UrlEncode(url);
                    filterContext.Result = new RedirectResult(redirectUrl, true);
                }
            }
            else
            {
                HttpContext.Current.Session["Token"] = token;
            }
        }

        /// <summary>
        /// 基於已驗證的用戶進行身份限制其存取，目前沒有實作
        /// </summary>
        /// <param name="filterContext">沒有使用</param>
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) {}

        private bool IsAnonymous(AuthenticationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(
                                                typeof(AllowAnonymousAttribute),
                                                true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                        typeof(AllowAnonymousAttribute),
                        true))
            {
                return false;
            }

            return true;
        }
    }
}