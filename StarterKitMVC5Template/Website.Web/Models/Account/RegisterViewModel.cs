using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website.Foundation.Aggregates;
using Website.Foundation.Container;
using Website.Foundation.Enums;
using $safeprojectname$.App_Start;
using $safeprojectname$.Codes;
using $safeprojectname$.Codes.Helper;
using $safeprojectname$.Codes.Service;

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

        public IUser CreateUser()
        {
            IMembershipService membershipService = NinjectWebCommon.GetConcreteInstance<IMembershipService>();
            IUser user = membershipService.CreateUser(new UserCreationData()
            {
                Email = this.Email,
                HasVerificationCode = true,
                Name = this.Email,
                Password = this.Password,
                TypeOfUser = UserType.Employee,
                UserName = this.Email
            });
            return user;
        }
        public void SendCofirmEmailIfRequired(IUser user)
        {
            if(!user.UserVerifications.Any())
                return;
            IUrlMakerHelper urlMakerHelper = NinjectWebCommon.GetConcreteInstance<IUrlMakerHelper>();
            IEmailService emailService = NinjectWebCommon.GetConcreteInstance<IEmailService>();
            string url = urlMakerHelper.GetUrlConfirmUser(user.UserVerifications.First().VerificationCode);
            emailService.SendConfirmUser(user, url);
        }
    }
}