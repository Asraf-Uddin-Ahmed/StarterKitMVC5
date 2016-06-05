using System;
using $safeprojectname$.Core.Aggregates;
namespace $safeprojectname$.Core.Services.Email
{
    // Director for MessageBuilder
    public interface IEmailService
    {
        void SendText(IMessageBuilder messageFactory);
        void SendHtml(IMessageBuilder messageFactory);
    }
}
