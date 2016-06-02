using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Services;
using $safeprojectname$.App_Start;
using $safeprojectname$.Codes.Core.Services;

namespace $safeprojectname$.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public void ProcessForgotPassword()
        {
            IMembershipService membershipService = NinjectWebCommon.GetConcreteInstance<IMembershipService>();
            IUserService userService = NinjectWebCommon.GetConcreteInstance<IUserService>();
            IUrlMakerService urlMakerHelper = NinjectWebCommon.GetConcreteInstance<IUrlMakerService>();
            IEmailService emailService = NinjectWebCommon.GetConcreteInstance<IEmailService>();

            User user = userService.GetUserByEmail(this.Email);
            PasswordVerification passwordVerification = membershipService.ProcessForgotPassword(user);
            string url = urlMakerHelper.GetUrlForgotPassword(passwordVerification.VerificationCode);
            emailService.SendForgotPassword(user, url);
        }
    }
}