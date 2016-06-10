using Microsoft.AspNet.Identity;
using Ninject;
using Ratul.Utility.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.Services.Email;
using $safeprojectname$.Persistence;
using $safeprojectname$.Persistence.Repositories;
using $safeprojectname$.Persistence.Services.Email;

namespace $safeprojectname$.Core.Identity
{
    public class EmailServiceProvider : IIdentityMessageService
    {
        private IEmailService _emailService;
        private ApplicationUserManager _applicationUserManager;
        private IIdentityMessageBuilder _identityMessageBuilder;
        [Inject]
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
