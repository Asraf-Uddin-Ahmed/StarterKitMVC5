namespace $safeprojectname$.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using $safeprojectname$.Aggregates;
    using $safeprojectname$.Enums;

    internal sealed class Configuration : DbMigrationsConfiguration<$safeprojectname$.TableContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "$safeprojectname$.TableContext";
        }

        protected override void Seed($safeprojectname$.TableContext context)
        {
            //List<Settings> listSettings = new List<Settings>
            //{
            //    new Settings(){DisplayName = "Max Password Mistake", Name = SettingsName.MaxPasswordMistake.ToString(), Type = SettingsType.Integer, Value = "5"},
            //    new Settings(){DisplayName = "Email Host", Name = SettingsName.EmailHost.ToString(), Type = SettingsType.String, Value = "smtp.gmail.com"},
            //    new Settings(){DisplayName = "Email User Name", Name = SettingsName.EmailUserName.ToString(), Type = SettingsType.String, Value = "ratulprojectinfo@gmail.com"},
            //    new Settings(){DisplayName = "Email Password", Name = SettingsName.EmailPassword.ToString(), Type = SettingsType.String, Value = "projectinfo"},
            //    new Settings(){DisplayName = "Email Port", Name = SettingsName.EmailPort.ToString(), Type = SettingsType.Integer, Value = "587"},
            //    new Settings(){DisplayName = "Email Enable SSL", Name = SettingsName.EmailEnableSSL.ToString(), Type = SettingsType.Boolean, Value = "true"},
            //    new Settings(){DisplayName = "System Email Address", Name = SettingsName.SystemEmailAddress.ToString(), Type = SettingsType.String, Value = "info@system.com"},
            //    new Settings(){DisplayName = "System Email Name", Name = SettingsName.SystemEmailName.ToString(), Type = SettingsType.String, Value = "System_Name"}
            //};
            //listSettings.ForEach(s => context.Settings.AddOrUpdate(p => p.ID, s));
            //context.SaveChanges();
        }
    }
}
