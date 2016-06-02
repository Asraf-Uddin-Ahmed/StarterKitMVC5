using System;
using System.Collections.Generic;
using Ninject.Modules;
using System.Web;
using System.Configuration;
using $safeprojectname$.Persistence;
using $safeprojectname$.Core.Factories;
using $safeprojectname$.Core.Services;
using $safeprojectname$.Persistence.Services;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Persistence.Repositories;
using $safeprojectname$.Persistence.Factories;

namespace $safeprojectname$
{
    public class NinjectFoundationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TableContext>().ToSelf();

            /*
             * REPOSITORY
             * */
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserVerificationRepository>().To<UserVerificationRepository>();
            Bind<IPasswordVerificationRepository>().To<PasswordVerificationRepository>();
            Bind<ISettingsRepository>().To<SettingsRepository>();

            /*
             * FACTORY
             * */
            Bind<IUserFactory>().To<UserFactory>();
            Bind<IUserVerificationFactory>().To<UserVerificationFactory>();
            Bind<IPasswordVerificationFactory>().To<PasswordVerificationFactory>();

            /*
             * SERVICE
             * */
            Bind<IUserService>().To<UserService>();
            Bind<IEmailService>().To<EmailService>();
            Bind<IPasswordVerificationService>().To<PasswordVerificationService>();
            Bind<IMembershipService>().To<MembershipService>();

        }
    }
}