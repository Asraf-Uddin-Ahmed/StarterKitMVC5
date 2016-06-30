using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Website.Identity.Providers;

namespace $safeprojectname$.App_Start
{
    public static class ExternalLoginConfig
    {
        public static void RegisterSignInCookie(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }

        public static GoogleOAuth2AuthenticationOptions RegisterGoogle(IAppBuilder app)
        {
            GoogleOAuth2AuthenticationOptions googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["google:ClientID"],
                ClientSecret = ConfigurationManager.AppSettings["google:ClientSecret"],
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);
            return googleAuthOptions;
        }

        public static FacebookAuthenticationOptions RegisterFacebook(IAppBuilder app)
        {
            FacebookAuthenticationOptions facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = ConfigurationManager.AppSettings["facebook:AppID"],
                AppSecret = ConfigurationManager.AppSettings["facebook:AppSecret"],
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);
            return facebookAuthOptions;
        }
    }
}