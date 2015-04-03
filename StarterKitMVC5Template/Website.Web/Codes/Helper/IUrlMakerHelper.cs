using System;
namespace $safeprojectname$.Codes.Helper
{
    public interface IUrlMakerHelper
    {
        string GetSiteUrl();
        string GetUrlConfirmUser(string varificationCode);
        string GetUrlForgotPassword(string varificationCode);
    }
}
