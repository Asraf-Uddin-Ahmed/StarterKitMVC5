using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Persistence.Repositories;

namespace $safeprojectname$.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        private IPasswordVerificationRepository _passwordVerifications;
        private ISettingsRepository _settings;
        private IUserRepository _users;
        private IUserVerificationRepository _userVerifications;

        [Inject]
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }



        public IPasswordVerificationRepository PasswordVerifications
        {
            get 
            {
                if(_passwordVerifications == null)
                {
                    _passwordVerifications = new PasswordVerificationRepository(_context);
                }
                return _passwordVerifications;
            }
        }
        public ISettingsRepository Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new SettingsRepository(_context);
                }
                return _settings;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new UserRepository(_context);
                }
                return _users;
            }
        }
        public IUserVerificationRepository UserVerifications
        {
            get
            {
                if (_userVerifications == null)
                {
                    _userVerifications = new UserVerificationRepository(_context);
                }
                return _userVerifications;
            }
        }



        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
