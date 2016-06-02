using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Core.Repositories
{
    public interface IUserVerificationRepository : IRepository<UserVerification>
    {
        UserVerification GetByVerificationCode(string verificationCode);
        bool IsVerificationCodeExist(string verificationCode);
        void RemoveByVerificationCode(string verificationCode, bool isPersist = false);
        void RemoveByUserID(Guid userID, bool isPersist = false);
    }
}
