using System;
using Website.Foundation.Aggregates;
namespace $safeprojectname$.Codes.Service
{
    public interface IMembershipService
    {
        bool BlockUser(Guid userID);
        bool ChangeUserPassword(Guid userID, string newPassword);
        bool ChangeUserPassword(Guid userID, string oldPassword, string newPassword);
        Website.Foundation.Aggregates.IUser CreateUser(Website.Foundation.Container.UserCreationData data);
        Website.Foundation.Enums.LoginStatus ProcessLogin(string userName, string password);
        bool UnblockUser(Guid userID);
        Website.Foundation.Enums.VerificationStatus VerifyForPasswordChange(string verificationCode);
        Website.Foundation.Enums.VerificationStatus VerifyForUserStatus(string verificationCode);
        IPasswordVerification ProcessForgotPassword(IUser user);
    }
}
