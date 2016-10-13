using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Foundation.Core.Aggregates.Identity;
using $safeprojectname$.Models;
namespace $safeprojectname$.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> AddRefreshToken(RefreshToken token);
        Client FindClient(string clientId);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        List<RefreshToken> GetAllRefreshTokens();
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);

        Task<IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>> FindAsync(UserLoginInfo loginInfo);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<IdentityResult> AddLoginAsync(Guid userId, UserLoginInfo login);
    }
}
