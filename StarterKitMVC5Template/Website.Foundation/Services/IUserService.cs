using System;
using System.Collections.Generic;
using $safeprojectname$.Aggregates;
namespace $safeprojectname$.Services
{
    public interface IUserService
    {
        bool DeleteUser(Guid userID);
        System.Collections.Generic.ICollection<$safeprojectname$.Aggregates.IUser> GetAllUserPaged(int pageNumber, int pageSize, Func<$safeprojectname$.Aggregates.IUser, dynamic> orderBy);
        $safeprojectname$.Aggregates.IUser GetUser(Guid userID);
        $safeprojectname$.Aggregates.IUser GetUserByEmail(string email);
        $safeprojectname$.Aggregates.IUser GetUserByUserName(string userName);
        bool IsEmailAddressAlreadyInUse(string email);
        bool IsUserNameAlreadyInUse(string userName);
        bool UpdateUserInformation($safeprojectname$.Aggregates.IUser user);
        ICollection<IUser> GetAll();
    }
}
