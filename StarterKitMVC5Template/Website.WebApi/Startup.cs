using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using $safeprojectname$.Configuration;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using $safeprojectname$.App_Start;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace $safeprojectname$
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }


        public void Configuration(IAppBuilder app)
        {
            ExternalLoginConfig.RegisterSignInCookie(app);
            FacebookAuthOptions = ExternalLoginConfig.RegisterFacebook(app);
            GoogleAuthOptions = ExternalLoginConfig.RegisterGoogle(app);

            OAuthServerOptions = OAuthTokenConfig.RegisterGeneration(app);
            OAuthTokenConfig.RegisterConsumption(app);

            WebApiConfig.Register(app);
        }

    }
}