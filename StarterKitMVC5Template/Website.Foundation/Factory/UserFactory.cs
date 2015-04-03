using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Enums;

namespace $safeprojectname$.Factory
{
    public class UserFactory : IUserFactory
    {
        public IUser CreateUser(string userName, string email, string password, string name, UserType type, UserStatus status)
        {
            IUser user = new User();
            user.Name = name;
            user.TypeOfUser = type;
            user.UserName = userName;
            user.EmailAddress = email;
            user.Status = status;
            user.EncryptedPassword = CryptographicUtility.Encrypt(password, user.ID);

            user.CreationTime = DateTime.UtcNow;
            user.LastLogin = null;
            user.LastWrongPasswordAttempt = null;
            user.UpdateTime = DateTime.UtcNow;
            user.WrongPasswordAttempt = 0;
            return user;
        }
    }
}
