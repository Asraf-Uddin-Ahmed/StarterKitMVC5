using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website.Foundation.Aggregates;
using Website.Foundation.Services;
using $safeprojectname$.App_Start;
using $safeprojectname$.Codes.Helper;
using $safeprojectname$.Codes.Service;

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
            IUrlMakerHelper urlMakerHelper = NinjectWebCommon.GetConcreteInstance<IUrlMakerHelper>();
            IEmailService emailService = NinjectWebCommon.GetConcreteInstance<IEmailService>();

            IUser user = userService.GetUserByEmail(this.Email);
            IPasswordVerification passwordVerification = membershipService.ProcessForgotPassword(user);
            string url = urlMakerHelper.GetUrlForgotPassword(passwordVerification.VerificationCode);
            emailService.SendForgotPassword(user, url);
        }
    }
}