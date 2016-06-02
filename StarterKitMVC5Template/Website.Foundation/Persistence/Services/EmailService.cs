using Ratul.Utility.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.Services;
using $safeprojectname$.Persistence.Template;

namespace $safeprojectname$.Persistence.Services
{
    public class EmailService : IEmailService
    {
        private EmailSender _emailSender;
        private ISettingsRepository _settingsService;
        public EmailService(ISettingsRepository settingsService)
        {
            _settingsService = settingsService;
            this.InitializeEmailSender();
        }

        private void InitializeEmailSender()
        {
            EmailSettings emailSettings = new EmailSettings();
            emailSettings.Host = _settingsService.GetValueByName(SettingsName.EmailHost);
            emailSettings.UserName = _settingsService.GetValueByName(SettingsName.EmailUserName);
            emailSettings.Password = _settingsService.GetValueByName(SettingsName.EmailPassword);
            emailSettings.Port = int.Parse(_settingsService.GetValueByName(SettingsName.EmailPort));
            emailSettings.EnableSsl = bool.Parse(_settingsService.GetValueByName(SettingsName.EmailEnableSSL));
            _emailSender = new EmailSender(emailSettings);
        }

        private void SendTextEmail(List<NameWithEmail> toList, NameWithEmail from, string subject, string body)
        {
            MessageSettings messageSettings = new MessageSettings();
            messageSettings.Subject = subject;
            messageSettings.Body = body;
            messageSettings.From = from;
            messageSettings.ReplyToList = new List<NameWithEmail>() { from };
            messageSettings.ToList = toList;
            messageSettings.IsBodyHtml = false;
            _emailSender.Send(messageSettings);
        }
        private void SendHtmlEmail(List<NameWithEmail> toList, NameWithEmail from, string subject, string body)
        {
            MessageSettings messageSettings = new MessageSettings();
            messageSettings.Subject = subject;
            messageSettings.Body = body;
            messageSettings.From = from;
            messageSettings.ReplyToList = new List<NameWithEmail>() { from };
            messageSettings.ToList = toList;
            messageSettings.IsBodyHtml = true;
            _emailSender.Send(messageSettings);
        }
        private void SendFromSystem(List<NameWithEmail> toList, string subject, string body)
        {
            string systemEmail = _settingsService.GetValueByName(SettingsName.SystemEmailAddress);
            string systemName = _settingsService.GetValueByName(SettingsName.SystemEmailName);
            NameWithEmail systemFrom = new NameWithEmail(systemName, systemEmail);
            SendTextEmail(toList, systemFrom, subject, body);
        }


        public void SendForgotPassword(User registeredUser, string url)
        {
            EmailForgotPassword template = new EmailForgotPassword(registeredUser, url);
            string body = template.TransformText();
            string subject = "Change your Website password";
            List<NameWithEmail> receiver = new List<NameWithEmail>() 
            { 
                new NameWithEmail(registeredUser.Name, registeredUser.EmailAddress) 
            };
            SendFromSystem(receiver, subject, body);
        }
        public void SendConfirmUser(User newUser, string url)
        {
            EmailConfirmUser template = new EmailConfirmUser(newUser, url);
            string body = template.TransformText();
            string subject = "Confirm your email address";
            List<NameWithEmail> receiver = new List<NameWithEmail>() 
            { 
                new NameWithEmail(newUser.Name, newUser.EmailAddress) 
            };
            SendFromSystem(receiver, subject, body);
        }
    }
}