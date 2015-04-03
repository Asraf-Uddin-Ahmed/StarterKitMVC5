using $safeprojectname$.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace $safeprojectname$.Repositories
{
    public class PasswordVerificationRepository : BaseEfRepository<PasswordVerification>, IPasswordVerificationRepository
    {
        private TableContext _context;
        [Inject]
        public PasswordVerificationRepository(TableContext context)
            : base(context)
        {
            _context = context;
        }

        public IPasswordVerification GetByVerificationCode(string verificationCode)
        {
            PasswordVerification passwordVerification = _context.PasswordVerifications.Where(col => col.VerificationCode == verificationCode).FirstOrDefault();
            return passwordVerification;
        }

        public bool IsVerificationCodeExist(string verificationCode)
        {
            bool isExist = _context.PasswordVerifications.Any(col => col.VerificationCode == verificationCode);
            return isExist;
        }

        public void RemoveByVerificationCode(string verificationCode)
        {
            IPasswordVerification passwordVerification = GetByVerificationCode(verificationCode);
            Remove(passwordVerification);
        }
        public void RemoveByUserID(Guid userID)
        {
            _context.PasswordVerifications.RemoveRange(_context.PasswordVerifications.Where(c => c.UserID == userID));
            _context.SaveChanges();
        }
    }
}
