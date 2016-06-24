using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Repositories;
using Website.Foundation.Core.Services.Email;
using Website.Foundation.Persistence;
using Website.Foundation.Persistence.Repositories;
using Website.Foundation.Persistence.Services.Email;
using $safeprojectname$.Message;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Providers;
using $safeprojectname$.Validators;

namespace $safeprojectname$.Managers
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            AuthDbContext authDbContext = context.Get<AuthDbContext>();
            ApplicationDbContext appDbContext = context.Get<ApplicationDbContext>();
            ApplicationUserManager appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(authDbContext));

            appUserManager.UserValidator = new CustomUserValidator(appUserManager);
            appUserManager.PasswordValidator = new CustomPasswordValidator();

            ConfigureEmailServiceProvider(appDbContext, appUserManager, options);

            return appUserManager;
        }


        private static void ConfigureEmailServiceProvider(ApplicationDbContext appDbContext, ApplicationUserManager appUserManager, IdentityFactoryOptions<ApplicationUserManager> options)
        {
            ISettingsRepository settingsRepository = new SettingsRepository(appDbContext);
            IIdentityMessageBuilder identityMessageBuilder = new IdentityMessageBuilder(settingsRepository);
            IEmailService emailService = new EmailService(settingsRepository);
            appUserManager.EmailService = new EmailServiceProvider(emailService, appUserManager, identityMessageBuilder);

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }
        }
    }
}