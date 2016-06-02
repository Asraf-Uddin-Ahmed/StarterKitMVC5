using System;
namespace $safeprojectname$.Codes.Core.Services
{
    public interface IUrlMakerService
    {
        string GetSiteUrl();
        string GetUrlConfirmUser(string varificationCode);
        string GetUrlForgotPassword(string varificationCode);
    }
}
