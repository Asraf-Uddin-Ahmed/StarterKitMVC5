using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Factories;

namespace $safeprojectname$.Persistence.Factories
{
    public class PasswordVerificationFactory : IPasswordVerificationFactory
    {
        public PasswordVerification Create()
        {
            PasswordVerification passwordVerification = new PasswordVerification();
            passwordVerification.ID = GuidUtility.GetNewSequentialGuid();
            passwordVerification.CreationTime = DateTime.UtcNow;
            passwordVerification.VerificationCode = UserUtility.GetNewVerificationCode();
            return passwordVerification;
        }
    }
}
