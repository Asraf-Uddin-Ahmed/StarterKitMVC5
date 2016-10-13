using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Foundation.Core.Aggregates.Identity;
using $safeprojectname$.Managers;
namespace $safeprojectname$.Helpers
{
    public interface IAuthHelper
    {
        Task<ClaimsIdentity> GetClaimIdentityAsync(ApplicationUser appUser, ApplicationUserManager appUserManager);
        AuthenticationProperties GetAuthenticationProperties(string userName, string clientID);
    }
}
