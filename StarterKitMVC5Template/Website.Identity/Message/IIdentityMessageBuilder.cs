using Website.Foundation.Core.Services.Email;
using Website.Foundation.Core.Aggregates.Identity;

namespace $safeprojectname$.Message
{
    public interface IIdentityMessageBuilder : IMessageBuilder
    {
        void Build(ApplicationUser user, string subject, string body);
    }
}
