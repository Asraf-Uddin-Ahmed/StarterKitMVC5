using System;
namespace $safeprojectname$.Factory
{
    public interface IUserFactory
    {
        $safeprojectname$.Aggregates.IUser CreateUser(string userName, string email, string password, string name, $safeprojectname$.Enums.UserType type, $safeprojectname$.Enums.UserStatus status);
    }
}
