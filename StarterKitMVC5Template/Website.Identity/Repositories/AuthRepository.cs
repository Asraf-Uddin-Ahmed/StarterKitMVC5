using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using $safeprojectname$;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Managers;
using $safeprojectname$.Models;

namespace $safeprojectname$.Repositories
{

    public class AuthRepository : IAuthRepository
    {
        private AuthDbContext _authDbContext;

        public AuthRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        
        public Client FindClient(string clientId)
        {
            var client = _authDbContext.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

           var existingToken = _authDbContext.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

           if (existingToken != null)
           {
             var result = await RemoveRefreshToken(existingToken);
           }
          
            _authDbContext.RefreshTokens.Add(token);

            return await _authDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
           var refreshToken = await _authDbContext.RefreshTokens.FindAsync(refreshTokenId);

           if (refreshToken != null) {
               _authDbContext.RefreshTokens.Remove(refreshToken);
               return await _authDbContext.SaveChangesAsync() > 0;
           }

           return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _authDbContext.RefreshTokens.Remove(refreshToken);
             return await _authDbContext.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _authDbContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
             return  _authDbContext.RefreshTokens.ToList();
        }
        
    }
}