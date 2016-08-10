using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ratul.Utility;
using System.IO;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.Aggregates;
using System.Linq.Expressions;
using $safeprojectname$.Core.Services;
using $safeprojectname$.Core.SearchData;

namespace $safeprojectname$.Persistence.Repositories
{
    public class UserRepository : RepositorySearch<User, UserSearch>, IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public bool IsUserNameExist(string userName)
        {
            bool isExist = _context.ExtendedUsers.Any(col => col.UserName == userName);
            return isExist;
        }
        public bool IsEmailExist(string email)
        {
            bool isExist = _context.ExtendedUsers.Any(col => col.EmailAddress == email);
            return isExist;
        }
        public User GetByUserName(string userName)
        {
            User user = _context.ExtendedUsers.FirstOrDefault(col => col.UserName == userName);
            return user;
        }
        public User GetByEmail(string email)
        {
            User user = _context.ExtendedUsers.FirstOrDefault(col => col.EmailAddress == email);
            return user;
        }



        protected override Expression<Func<User, bool>> GetAndSearchCondition(UserSearch searchItem)
        {
            Expression<Func<User, bool>> predicate = (col) =>
                (searchItem.EmailAddress == null || searchItem.EmailAddress == col.EmailAddress)
                && (searchItem.UserName == null || searchItem.UserName == col.UserName)
                && (searchItem.TypeOfUser == null || searchItem.TypeOfUser == col.TypeOfUser)
                && (searchItem.Status == null || searchItem.Status == col.Status)
                && (searchItem.WrongPasswordAttempt == null || searchItem.WrongPasswordAttempt == col.WrongPasswordAttempt)
                && (searchItem.LastWrongPasswordAttempt == null || searchItem.LastWrongPasswordAttempt == col.LastWrongPasswordAttempt)
                && (searchItem.CreationTime == null || searchItem.CreationTime == col.CreationTime)
                && (searchItem.UpdateTime == null || searchItem.UpdateTime == col.UpdateTime);
            return predicate;
        }
        protected override Expression<Func<User, bool>> GetOrSearchCondition(UserSearch searchItem)
        {
            bool isAllNull = base.IsAllPropertyNull(searchItem);
            Expression<Func<User, bool>> predicate = (col) =>
                (searchItem.EmailAddress != null && searchItem.EmailAddress == col.EmailAddress)
                || (searchItem.UserName != null && searchItem.UserName == col.UserName)
                || (searchItem.TypeOfUser != null && searchItem.TypeOfUser == col.TypeOfUser)
                || (searchItem.Status != null && searchItem.Status == col.Status)
                || (searchItem.WrongPasswordAttempt != null && searchItem.WrongPasswordAttempt == col.WrongPasswordAttempt)
                || (searchItem.LastWrongPasswordAttempt != null && searchItem.LastWrongPasswordAttempt == col.LastWrongPasswordAttempt)
                || (searchItem.CreationTime != null && searchItem.CreationTime == col.CreationTime)
                || (searchItem.UpdateTime != null && searchItem.UpdateTime == col.UpdateTime)
                || isAllNull;
            return predicate;
        }
        
    }
}
