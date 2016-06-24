using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Constants;
using $safeprojectname$.Helpers;
using $safeprojectname$.Repositories;
using Microsoft.AspNet.Identity.Owin;
using $safeprojectname$.Managers;

namespace $safeprojectname$.Providers
{
    public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary[AuthenticationPropertyKeys.CLIENT_ID];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            AuthDbContext authDbContext = context.OwinContext.Get<AuthDbContext>();
            AuthRepository _repo = new AuthRepository(authDbContext);
            var refreshTokenLifeTime = context.OwinContext.Get<string>(OwinContextKeys.CLIENT_REFRESH_TOKEN_LIFE_TIME);

            var token = new RefreshToken()
            {
                Id = HashGenerator.GetHash(refreshTokenId),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await _repo.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }

        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(OwinContextKeys.CLIENT_ALLOWED_ORIGIN);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = HashGenerator.GetHash(context.Token);

            AuthDbContext authDbContext = context.OwinContext.Get<AuthDbContext>();
            AuthRepository _repo = new AuthRepository(authDbContext);
            var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await _repo.RemoveRefreshToken(hashedTokenId);
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}