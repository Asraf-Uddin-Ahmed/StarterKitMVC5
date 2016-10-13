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
using Website.Foundation.Core.Aggregates.Identity;
using $safeprojectname$.Managers;
using $safeprojectname$.Models;
using Website.Foundation.Persistence;

namespace $safeprojectname$.Repositories
{

    public class AuthRepository : IAuthRepository
    {
        private ApplicationDbContext _appDbContext;
        private ApplicationUserManager _applicationUserManager;
        public AuthRepository(ApplicationDbContext appDbContext, ApplicationUserManager applicationUserManager)
        {
            _appDbContext = appDbContext;
            _applicationUserManager = applicationUserManager;
        }


        public Client FindClient(string clientId)
        {
            var client = _appDbContext.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _appDbContext.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _appDbContext.RefreshTokens.Add(token);

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _appDbContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _appDbContext.RefreshTokens.Remove(refreshToken);
                return await _appDbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _appDbContext.RefreshTokens.Remove(refreshToken);
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _appDbContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _appDbContext.RefreshTokens.ToList();
        }

        public async Task<IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim> user = await _applicationUserManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await _applicationUserManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(Guid userId, UserLoginInfo login)
        {
            var result = await _applicationUserManager.AddLoginAsync(userId, login);

            return result;
        }
    }
}