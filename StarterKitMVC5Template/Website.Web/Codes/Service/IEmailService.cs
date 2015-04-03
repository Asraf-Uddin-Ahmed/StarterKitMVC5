using System;
namespace $safeprojectname$.Codes.Service
{
    public interface IEmailService
    {
        void SendConfirmUser(Website.Foundation.Aggregates.IUser newUser, string url);
        void SendForgotPassword(Website.Foundation.Aggregates.IUser registeredUser, string url);
    }
}
