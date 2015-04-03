using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website.Foundation.Enums;
using $safeprojectname$.App_Start;
using $safeprojectname$.Codes.Service;

namespace $safeprojectname$.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public void StoreActionResponseMessageByLoginStatus(LoginStatus status)
        {
            IValidationMessageService validationMessageService = NinjectWebCommon.GetConcreteInstance<IValidationMessageService>();
            if (status == LoginStatus.Blocked)
                validationMessageService.StoreActionResponseMessageInfo("Your account has been blocked. Please contact with support.");
            else if(status == LoginStatus.Failed)
                validationMessageService.StoreActionResponseMessageError("Problem has been occurred while proccessing you requst. Please try again.");
            else if(status == LoginStatus.InvalidLogin)
                validationMessageService.StoreActionResponseMessageError("Incorrect Username or Password");
            else if(status == LoginStatus.Successful)
                validationMessageService.StoreActionResponseMessageSuccess("Login Successful");
            else if(status == LoginStatus.Unverified)
                validationMessageService.StoreActionResponseMessageError("Please confirm your email address.");
        }
    }
}