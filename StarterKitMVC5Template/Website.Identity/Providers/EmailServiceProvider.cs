using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Foundation.Core.Services.Email;
using $safeprojectname$.Managers;
using $safeprojectname$.Message;
using $safeprojectname$.Aggregates;

namespace $safeprojectname$.Providers
{
    public class EmailServiceProvider : IIdentityMessageService
    {
        private IEmailService _emailService;
        private ApplicationUserManager _applicationUserManager;
        private IIdentityMessageBuilder _identityMessageBuilder;
        public EmailServiceProvider(IEmailService emailService,
            ApplicationUserManager applicationUserManager,
            IIdentityMessageBuilder identityMessageBuilder)
        {
            _emailService = emailService;
            _applicationUserManager = applicationUserManager;
            _identityMessageBuilder = identityMessageBuilder;
        }

        public Task SendAsync(IdentityMessage message)
        {
            return Task.Run(() =>
                {
                    ApplicationUser user = _applicationUserManager.FindByEmail(message.Destination);
                    _identityMessageBuilder.Build(user, message.Subject, message.Body);
                    _emailService.SendHtmlAsync(_identityMessageBuilder);
                });
        }


    }
}
