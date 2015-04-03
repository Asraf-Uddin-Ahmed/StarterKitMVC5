using System;
using $safeprojectname$.Aggregates;
namespace $safeprojectname$.Repositories
{
    public interface IUserRepository : IBaseEfRepository<User>
    {
        System.Collections.Generic.IEnumerable<$safeprojectname$.Aggregates.IUser> GetPagedAnd($safeprojectname$.Container.UserSearch searchItem, int pageNumber, int pageSize, Func<$safeprojectname$.Aggregates.IUser, dynamic> predicateOrderBy);
        System.Collections.Generic.IEnumerable<$safeprojectname$.Aggregates.IUser> GetPagedOr($safeprojectname$.Container.UserSearch searchItem, int pageNumber, int pageSize, Func<$safeprojectname$.Aggregates.IUser, dynamic> predicateOrderBy);
        int GetTotalAnd($safeprojectname$.Container.UserSearch searchItem);
        int GetTotalOr($safeprojectname$.Container.UserSearch searchItem);
        bool IsEmailExist(string email);
        bool IsUserNameExist(string userName);
        IUser GetByUserName(string userName);
        IUser GetByEmail(string email);
    }
}
