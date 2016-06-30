using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Website.Identity.Aggregates;
using Website.Identity.Constants;
using Website.Identity.Helpers;
using Website.Identity.Managers;
using Website.Identity.Models;
using Website.Identity.Providers;
using Website.Identity.Repositories;
using $safeprojectname$.Configuration.Identity;

namespace $safeprojectname$.Controllers.Identity
{
    [RoutePrefix("api/account/external")]
    public class ExternalAccountController : IdentityApiController
    {
        private ILogger _logger;
        private ApplicationUserManager _applicationUserManager;
        private IAuthRepository _authRepository;
        private IAuthHelper _authHelper;

        private IAuthenticationManager OwinAuthentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public ExternalAccountController(ILogger logger,
            IAuthRepository authRepository,
            IAuthHelper authHelper,
            ApplicationUserManager applicationUserManager)
            : base(logger)
        {
            _logger = logger;
            _authRepository = authRepository;
            _authHelper = authHelper;
            _applicationUserManager = applicationUserManager;
        }


        // GET api/account/external/Login
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("Login", Name = "ExternalLogin")]
        [HttpGet]
        public async Task<IHttpActionResult> Login(ExternalLoginProviderName provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                this.OwinAuthentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await _authRepository.FindAsync(new UserLoginInfo(externalLogin.LoginProvider.ToString(), externalLogin.ProviderKey));
            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }

        // POST api/account/external/Register
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _authRepository.FindAsync(new UserLoginInfo(model.Provider.ToString(), verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            ApplicationUser appUser = new ApplicationUser() { UserName = model.UserName };

            IdentityResult result = await _authRepository.CreateAsync(appUser);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider.ToString(), verifiedAccessToken.user_id)
            };

            result = await _authRepository.AddLoginAsync(appUser.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            var accessTokenResponse = await GenerateLocalAccessTokenResponse(model.UserName, model.ClientID);

            return Ok(accessTokenResponse);
        }

        //GET api/account/external/ObtainLocalAccessToken
        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken([FromUri]ExternalLocalAccessToken model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _authRepository.FindAsync(new UserLoginInfo(model.Provider.ToString(), verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = await GenerateLocalAccessTokenResponse(user.UserName, model.ClientID);

            return Ok(accessTokenResponse);

        }

        //GET api/account/external/Signout
        [AllowAnonymous]
        [Route("Signout")]
        [HttpGet]
        public IHttpActionResult Signout()
        {
            this.OwinAuthentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            return Ok();
        }


        #region Helpers

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _authRepository.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            if (client.AllowedOrigin != "*" && !string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            }

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(ExternalLoginProviderName provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == ExternalLoginProviderName.Facebook)
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = ConfigurationManager.AppSettings["facebook:AppToken"];
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == ExternalLoginProviderName.Google)
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == ExternalLoginProviderName.Facebook)
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.FacebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == ExternalLoginProviderName.Google)
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.GoogleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        private async Task<JObject> GenerateLocalAccessTokenResponse(string userName, string clientID)
        {
            ApplicationUser user = await _applicationUserManager.FindByNameAsync(userName);
            ClaimsIdentity oAuthIdentity = await _authHelper.GetClaimIdentityAsync(user, _applicationUserManager);

            TimeSpan tokenExpiration = Startup.OAuthServerOptions.AccessTokenExpireTimeSpan;
            AuthenticationProperties props = _authHelper.GetAuthenticationProperties(userName, clientID);
            props.IssuedUtc = DateTime.UtcNow;
            props.ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, props);
            string accessToken = Startup.OAuthServerOptions.AccessTokenFormat.Protect(ticket);

            AuthenticationTokenCreateContext context = new AuthenticationTokenCreateContext(Request.GetOwinContext(), Startup.OAuthServerOptions.AccessTokenFormat, ticket);
            await Startup.OAuthServerOptions.RefreshTokenProvider.CreateAsync(context);

            JObject tokenResponse = new JObject(
                new JProperty("userName", userName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("refresh_token", context.Token),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
                );
            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public ExternalLoginProviderName LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = (ExternalLoginProviderName)Enum.Parse(typeof(ExternalLoginProviderName), providerKeyClaim.Issuer, true),
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion

    }
}
