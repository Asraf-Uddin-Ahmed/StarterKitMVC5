using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Repositories;

namespace $safeprojectname$.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IPasswordVerificationRepository PasswordVerifications { get; }
        ISettingsRepository Settings { get; }
        IUserRepository Users { get; }
        IUserVerificationRepository UserVerifications { get; }


        void Commit();
    }
}
