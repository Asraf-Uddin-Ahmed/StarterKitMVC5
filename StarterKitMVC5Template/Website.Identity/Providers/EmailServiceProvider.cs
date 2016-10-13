using Website.Foundation.Core.Services.Email;
using Website.Foundation.Core.Aggregates.Identity;
using $safeprojectname$.Managers;
using $safeprojectname$.Message;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

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
