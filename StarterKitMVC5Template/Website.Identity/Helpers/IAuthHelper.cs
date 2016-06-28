using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Managers;
namespace $safeprojectname$.Helpers
{
    public interface IAuthHelper
    {
        Task<ClaimsIdentity> GetClaimIdentityAsync(ApplicationUser appUser, ApplicationUserManager appUserManager);
        AuthenticationProperties GetAuthenticationProperties(string userName, string clientID);
    }
}
