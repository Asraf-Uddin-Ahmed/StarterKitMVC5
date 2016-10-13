using Website.Foundation.Core.Aggregates.Identity;
using Website.Foundation.Core.Enums;
using Website.Foundation.Persistence;
using Website.Foundation.Persistence.Services;
using $safeprojectname$.Constants;
using $safeprojectname$.Helpers;
using $safeprojectname$.Managers;
using $safeprojectname$.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace $safeprojectname$.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError(ErrorType.INVALID_CLIENT_ID, "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            ApplicationDbContext appDbContext = context.OwinContext.Get<ApplicationDbContext>();
            ApplicationUserManager applicationUserManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            AuthRepository _repo = new AuthRepository(appDbContext, applicationUserManager);
            client = _repo.FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError(ErrorKeys.INVALID_CLIENT_ID, string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError(ErrorKeys.INVALID_CLIENT_ID, "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != HashGenerator.GetHash(clientSecret))
                    {
                        context.SetError(ErrorKeys.INVALID_CLIENT_ID, "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError(ErrorKeys.INVALID_CLIENT_ID, "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>(OwinContextKeys.CLIENT_ALLOWED_ORIGIN, client.AllowedOrigin);

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(OwinContextKeys.CLIENT_ALLOWED_ORIGIN);
            allowedOrigin = allowedOrigin == null ? "*" : allowedOrigin;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError(ErrorKeys.INVALID_GRANT, "The user name or password is incorrect.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError(ErrorKeys.INVALID_GRANT, "User did not confirm email.");
                return;
            }

            AuthHelper authHelper = new AuthHelper();
            ClaimsIdentity oAuthIdentity = await authHelper.GetClaimIdentityAsync(user, userManager);
            var props = authHelper.GetAuthenticationProperties(context.UserName, context.ClientId);
            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            ticket.Properties.Dictionary.Add("userID", user.Id.ToString());
            context.Validated(ticket);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary[AuthenticationPropertyKeys.CLIENT_ID];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError(ErrorKeys.INVALID_CLIENT_ID, "Refresh token is issued to a different clientId.");
                return;
            }

            // Change auth ticket for refresh token requests
            ClaimsIdentity currentIdentity = new ClaimsIdentity(context.Ticket.Identity);
            ApplicationUserManager userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            // Get up-to-date user with claims and roles for creating new ticket
            ApplicationUser user = await userManager.FindByNameAsync(currentIdentity.Name);
            AuthHelper authHelper = new AuthHelper();
            ClaimsIdentity oAuthIdentity = await authHelper.GetClaimIdentityAsync(user, userManager);

            var newTicket = new AuthenticationTicket(oAuthIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}