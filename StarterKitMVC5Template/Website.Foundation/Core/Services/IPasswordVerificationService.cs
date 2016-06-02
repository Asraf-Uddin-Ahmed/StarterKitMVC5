using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Core.Services
{
    public interface IPasswordVerificationService
    {
        void RemoveByUserID(Guid userID);
    }
}
