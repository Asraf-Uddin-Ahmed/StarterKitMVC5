using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using $safeprojectname$.Managers;
using Website.Foundation.Core.Aggregates.Identity;

namespace $safeprojectname$.Validators
{
    public class CustomUserValidator : UserValidator<ApplicationUser, Guid>
    {
        public CustomUserValidator(ApplicationUserManager appUserManager)
            : base(appUserManager)
        {
            base.AllowOnlyAlphanumericUserNames = false;
            base.RequireUniqueEmail = true;
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            //var emailDomain = user.Email.Split('@')[1];

            //List<string> _allowedEmailDomains = new List<string> { "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };
            //if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
            //{
            //    var errors = result.Errors.ToList();

            //    errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

            //    result = new IdentityResult(errors);
            //}

            return result;
        }
    }
}