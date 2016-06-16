using System;
using System.Collections.Generic;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.SearchData;
namespace $safeprojectname$.Core.Repositories
{
    public interface IUserRepository : IRepositorySearch<User, UserSearch>
    {
        bool IsEmailExist(string email);
        bool IsUserNameExist(string userName);
        User GetByUserName(string userName);
        User GetByEmail(string email);
    }
}
