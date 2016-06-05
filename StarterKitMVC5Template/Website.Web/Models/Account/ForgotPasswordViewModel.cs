using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Services;
using Website.Foundation.Core.Services.Email;
using $safeprojectname$.App_Start;
using $safeprojectname$.Codes.Core.Services;
using $safeprojectname$.Codes.Core.Services.UriMaker;

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
            IUriMakerService uriMakerService = NinjectWebCommon.GetConcreteInstance<IUriMakerService>();
            IForgotPasswordUriBuilder forgotPasswordUriBuilder = NinjectWebCommon.GetConcreteInstance<IForgotPasswordUriBuilder>();
            IEmailService emailService = NinjectWebCommon.GetConcreteInstance<IEmailService>();
            IForgotPasswordMessageBuilder forgotPasswordMessageBuilder = NinjectWebCommon.GetConcreteInstance<IForgotPasswordMessageBuilder>();

            User user = userService.GetUserByEmail(this.Email);
            PasswordVerification passwordVerification = membershipService.ProcessForgotPassword(user);
            forgotPasswordUriBuilder.Build(passwordVerification.VerificationCode);
            string url = uriMakerService.GetFullUri(forgotPasswordUriBuilder);
            forgotPasswordMessageBuilder.Build(user, url);
            emailService.SendText(forgotPasswordMessageBuilder);
        }
    }
}