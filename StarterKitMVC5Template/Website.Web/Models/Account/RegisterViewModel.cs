using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Enums;
using Website.Foundation.Core.Factories;
using Website.Foundation.Core.Services;
using Website.Foundation.Core.Services.Email;
using Website.Foundation.Persistence.Services.Email;
using $safeprojectname$.App_Start;
using $safeprojectname$.Codes;
using $safeprojectname$.Codes.Core.Services;
using $safeprojectname$.Codes.Core.Services.UriMaker;

namespace $safeprojectname$.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public User CreateUser()
        {
            IMembershipService membershipService = NinjectWebCommon.GetConcreteInstance<IMembershipService>();
            IUserFactory userFactory = NinjectWebCommon.GetConcreteInstance<IUserFactory>();
            User user = userFactory.Create(this.Password);
            user.EmailAddress = this.Email;
            user.UserName = this.Email;
            user.Name = this.Email;
            user.Status = UserStatus.Unverified;
            user.TypeOfUser = UserType.Employee;
            user = membershipService.CreateUser(user);
            return user;
        }
        public void SendCofirmEmailIfRequired(User user)
        {
            if(!user.UserVerifications.Any())
                return;
            IUriMakerService uriMakerService = NinjectWebCommon.GetConcreteInstance<IUriMakerService>();
            IConfirmUserUriBuilder confirmUserUriBuilder = NinjectWebCommon.GetConcreteInstance<IConfirmUserUriBuilder>();
            IEmailService emailService = NinjectWebCommon.GetConcreteInstance<IEmailService>();
            IConfirmUserMessageBuilder forgotPasswordMessageBuilder = NinjectWebCommon.GetConcreteInstance<IConfirmUserMessageBuilder>();

            confirmUserUriBuilder.Build(user.UserVerifications.First().VerificationCode);
            string url = uriMakerService.GetFullUri(confirmUserUriBuilder);
            forgotPasswordMessageBuilder.Build(user, url);
            emailService.SendText(forgotPasswordMessageBuilder);
        }
    }
}