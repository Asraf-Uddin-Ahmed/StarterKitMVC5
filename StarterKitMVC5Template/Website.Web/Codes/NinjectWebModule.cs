using System;
using System.Collections.Generic;
using Ninject.Modules;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.Web.Hosting;
using System.Configuration;
using Ratul.Utility;
using $safeprojectname$.Codes.Persistence.Services;
using $safeprojectname$.Codes.Core.Services;

namespace $safeprojectname$.Codes
{
    public class NinjectWebModule : NinjectModule
    {
        public override void Load()
        {
            /*
             * MISCELLANEOUS
             * */
            Bind<RegexUtility>().ToSelf();

            /*
             * SERVICE
             * */
            Bind<IValidationMessageService>().To<ValidationMessageService>();
            Bind<IUrlMakerService>().To<UrlMakerService>();
            
        }
    }
}