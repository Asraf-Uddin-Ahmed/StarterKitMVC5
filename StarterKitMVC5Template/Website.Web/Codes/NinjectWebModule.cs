using System;
using System.Collections.Generic;
using Ninject.Modules;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.Web.Hosting;
using System.Configuration;
using Ratul.Utility;
using $safeprojectname$.Codes.Service;
using $safeprojectname$.Codes.Helper;

namespace $safeprojectname$.Codes
{
    public class NinjectWebModule : NinjectModule
    {
        public override void Load()
        {
            // OTHER
            Bind<IRegexUtility>().To<RegexUtility>();

            // SERVICE
            Bind<IMembershipService>().To<MembershipService>();
            Bind<IValidationMessageService>().To<ValidationMessageService>();
            Bind<IEmailService>().To<EmailService>();
            
            // HELPER
            Bind<IUrlMakerHelper>().To<UrlMakerHelper>();
            
        }
    }
}