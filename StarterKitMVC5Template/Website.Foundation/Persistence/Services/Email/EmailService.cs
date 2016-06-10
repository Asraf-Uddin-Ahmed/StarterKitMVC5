using Ratul.Utility.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.Services.Email;
using $safeprojectname$.Persistence.Template;

namespace $safeprojectname$.Persistence.Services.Email
{
    public class EmailService : IEmailService
    {
        private EmailSender _emailSender;
        private ISettingsRepository _settingsRepository;
        
        
        public EmailService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            this.InitializeEmailSender();
        }



        public void SendText(IMessageBuilder messageBuilder)
        {
            _emailSender.Send(messageBuilder.GetText());
        }
        public void SendHtml(IMessageBuilder messageBuilder)
        {
            _emailSender.Send(messageBuilder.GetHtml());
        }
        public void SendTextAsync(IMessageBuilder messageBuilder)
        {
            _emailSender.SendAsync(messageBuilder.GetText());
        }
        public void SendHtmlAsync(IMessageBuilder messageBuilder)
        {
            _emailSender.SendAsync(messageBuilder.GetHtml());
        }
        public void SendTextAsync(IMessageBuilder messageBuilder, EmailSender.SendCompletedCallback sendCompletedCallback)
        {
            _emailSender.SendAsync(messageBuilder.GetText(), sendCompletedCallback);
        }
        public void SendHtmlAsync(IMessageBuilder messageBuilder, EmailSender.SendCompletedCallback sendCompletedCallback)
        {
            _emailSender.SendAsync(messageBuilder.GetHtml(), sendCompletedCallback);
        }
        public void SendAsyncCancel()
        {
            _emailSender.SendAsyncCancel();
        }


        private void InitializeEmailSender()
        {
            EmailSettings emailSettings = new EmailSettings();
            emailSettings.Host = _settingsRepository.GetValueByName(SettingsName.EmailHost);
            emailSettings.UserName = _settingsRepository.GetValueByName(SettingsName.EmailUserName);
            emailSettings.Password = _settingsRepository.GetValueByName(SettingsName.EmailPassword);
            emailSettings.Port = int.Parse(_settingsRepository.GetValueByName(SettingsName.EmailPort));
            emailSettings.EnableSsl = bool.Parse(_settingsRepository.GetValueByName(SettingsName.EmailEnableSSL));
            _emailSender = new EmailSender(emailSettings);
        }

    }
}