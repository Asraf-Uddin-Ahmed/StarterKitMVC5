using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Aggregates;

namespace $safeprojectname$.Repositories
{
    public interface IUserVerificationRepository : IBaseEfRepository<UserVerification>
    {
        IUserVerification GetByVerificationCode(string verificationCode);
        bool IsVerificationCodeExist(string verificationCode);
        void RemoveByVerificationCode(string verificationCode);
        void RemoveByUserID(Guid userID);
    }
}
