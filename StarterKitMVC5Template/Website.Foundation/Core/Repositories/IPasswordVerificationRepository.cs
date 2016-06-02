using System;
using $safeprojectname$.Core.Aggregates;
namespace $safeprojectname$.Core.Repositories
{
    public interface IPasswordVerificationRepository : IRepository<PasswordVerification>
    {
        PasswordVerification GetByVerificationCode(string verificationCode);
        bool IsVerificationCodeExist(string verificationCode);
        void RemoveByUserID(Guid userID);
        void RemoveByVerificationCode(string verificationCode);
    }
}
