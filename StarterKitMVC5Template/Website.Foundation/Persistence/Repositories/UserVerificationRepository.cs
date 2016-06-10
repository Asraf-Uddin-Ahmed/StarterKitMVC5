using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Repositories;

namespace $safeprojectname$.Persistence.Repositories
{
    public class UserVerificationRepository : Repository<UserVerification>, IUserVerificationRepository
    {
        private ApplicationDbContext _context;
        [Inject]
        public UserVerificationRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public UserVerification GetByVerificationCode(string verificationCode)
        {
            UserVerification userVerification = _context.UserVerifications.Where(col => col.VerificationCode == verificationCode).FirstOrDefault();
            return userVerification;
        }

        public bool IsVerificationCodeExist(string verificationCode)
        {
            bool isExist = _context.UserVerifications.Any(col => col.VerificationCode == verificationCode);
            return isExist;
        }

        public void RemoveByVerificationCode(string verificationCode)
        {
            UserVerification userVerification = GetByVerificationCode(verificationCode);
            base.Remove(userVerification);
        }
        public void RemoveByUserID(Guid userID)
        {
            base.RemoveRange(_context.UserVerifications.Where(c => c.UserID == userID));
        }
    }
}
