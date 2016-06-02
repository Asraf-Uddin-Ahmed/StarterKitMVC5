using System;
using $safeprojectname$.Core.Aggregates;
namespace $safeprojectname$.Core.Services
{
    public interface IMembershipService
    {
        bool BlockUser(Guid userID);
        bool ChangeUserPassword(Guid userID, string newPassword);
        bool ChangeUserPassword(Guid userID, string oldPassword, string newPassword);
        User CreateUser(User user);
        $safeprojectname$.Core.Enums.LoginStatus ProcessLogin(string userName, string password);
        bool UnblockUser(Guid userID);
        $safeprojectname$.Core.Enums.VerificationStatus VerifyForPasswordChange(string verificationCode);
        $safeprojectname$.Core.Enums.VerificationStatus VerifyForUserStatus(string verificationCode);
        PasswordVerification ProcessForgotPassword(User user);
    }
}
