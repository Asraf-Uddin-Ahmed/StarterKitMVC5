using $safeprojectname$.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using $safeprojectname$.Container;
using $safeprojectname$.Helpers;
using Ratul.Utility;
using System.IO;

namespace $safeprojectname$.Repositories
{
    public class UserRepository : BaseEfRepository<User>, IUserRepository
    {
        private TableContext _context;
        private IRepositorySearchHelper _repositorySearchHelper;
        [Inject]
        public UserRepository(TableContext context, IRepositorySearchHelper repositorySearchHelper)
            : base(context)
        {
            _context = context;
            _repositorySearchHelper = repositorySearchHelper;
        }

        public bool IsUserNameExist(string userName)
        {
            bool isExist = _context.Users.Any(col => col.UserName == userName);
            return isExist;
        }
        public bool IsEmailExist(string email)
        {
            bool isExist = _context.Users.Any(col => col.EmailAddress == email);
            return isExist;
        }
        public IUser GetByUserName(string userName)
        {
            IUser user = _context.Users.FirstOrDefault(col => col.UserName == userName);
            return user;
        }
        public IUser GetByEmail(string email)
        {
            IUser user = _context.Users.FirstOrDefault(col => col.EmailAddress == email);
            return user;
        }

        private Func<IUser, bool> GetAndSearchCondition(UserSearch searchItem)
        {
            Func<IUser, bool> predicate = (col) =>
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
        private Func<IUser, bool> GetOrSearchCondition(UserSearch searchItem)
        {
            bool isAllNull = _repositorySearchHelper.IsAllPropertyNull(searchItem);
            Func<IUser, bool> predicate = (col) =>
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
        public int GetTotalAnd(UserSearch searchItem)
        {
            Func<IUser, bool> predicateCount = GetAndSearchCondition(searchItem);
            int total = base.GetTotalBy(predicateCount);
            return total;
        }
        public int GetTotalOr(UserSearch searchItem)
        {
            Func<IUser, bool> predicateCount = GetOrSearchCondition(searchItem);
            int total = base.GetTotalBy(predicateCount);
            return total;
        }

        public IEnumerable<IUser> GetPagedAnd(UserSearch searchItem, int pageNumber, int pageSize, Func<IUser, dynamic> predicateOrderBy)
        {
            Func<IUser, bool> predicateWhere = GetAndSearchCondition(searchItem);
            IEnumerable<IEntity> listUser = base.GetPagedBy(pageNumber, pageSize, predicateOrderBy, predicateWhere);
            return listUser.Cast<IUser>();
        }
        public IEnumerable<IUser> GetPagedOr(UserSearch searchItem, int pageNumber, int pageSize, Func<IUser, dynamic> predicateOrderBy)
        {
            Func<IUser, bool> predicateWhere = GetOrSearchCondition(searchItem);
            IEnumerable<IEntity> listUser = base.GetPagedBy(pageNumber, pageSize, predicateOrderBy, predicateWhere);
            return listUser.Cast<IUser>();
        }
    }
}
