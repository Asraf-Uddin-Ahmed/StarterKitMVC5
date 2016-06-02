using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Codes.Core.Services;

namespace $safeprojectname$.Codes.Persistence.Services
{
    public class UrlMakerService : IUrlMakerService
    {
        /// <summary>
        /// Get root url of site.
        /// </summary>
        /// <returns></returns>
        public string GetSiteUrl()
        {
            string authority = HttpContext.Current.Request.Url.Authority;
            string scheme = HttpContext.Current.Request.Url.Scheme;
            string siteUrl = scheme + "://" + authority;
            return siteUrl;
        }

        public string GetUrlForgotPassword(string varificationCode)
        {
            string forgotPasswordUrl = GetSiteUrl() + "/Account/ChangePassword?code=" + varificationCode;
            return forgotPasswordUrl;
        }

        public string GetUrlConfirmUser(string varificationCode)
        {
            string forgotPasswordUrl = GetSiteUrl() + "/Account/ConfirmUser?code=" + varificationCode;
            return forgotPasswordUrl;
        }

    }
}