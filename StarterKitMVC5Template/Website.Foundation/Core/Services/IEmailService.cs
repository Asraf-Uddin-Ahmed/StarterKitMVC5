using System;
using $safeprojectname$.Core.Aggregates;
namespace $safeprojectname$.Core.Services
{
    public interface IEmailService
    {
        void SendConfirmUser(User newUser, string url);
        void SendForgotPassword(User registeredUser, string url);
    }
}
