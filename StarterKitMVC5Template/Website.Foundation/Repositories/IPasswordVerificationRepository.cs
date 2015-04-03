using System;
using $safeprojectname$.Aggregates;
namespace $safeprojectname$.Repositories
{
    public interface IPasswordVerificationRepository : IBaseEfRepository<PasswordVerification>
    {
        $safeprojectname$.Aggregates.IPasswordVerification GetByVerificationCode(string verificationCode);
        bool IsVerificationCodeExist(string verificationCode);
        void RemoveByUserID(Guid userID);
        void RemoveByVerificationCode(string verificationCode);
    }
}
