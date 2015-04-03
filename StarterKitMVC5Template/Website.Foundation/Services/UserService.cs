using Newtonsoft.Json;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Container;
using $safeprojectname$.Repositories;

namespace $safeprojectname$.Services
{
    public class UserService : IUserService
    {
        private ILogger _logger;
        private IUserRepository _userRepository;
        [Inject]
        public UserService(ILogger logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public bool IsEmailAddressAlreadyInUse(string email)
        {
            try
            {
                bool isExist = _userRepository.IsEmailExist(email);
                return isExist;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to check email address: email={0}", email);
                return false;
            }
        }

        public bool IsUserNameAlreadyInUse(string userName)
        {
            try
            {
                bool isExist = _userRepository.IsUserNameExist(userName);
                return isExist;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get user by username with parameters: username={0}", userName);
                return false;
            }
        }

        public IUser GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("Username is missing");

            try
            {
                return _userRepository.GetByUserName(userName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get user by username with UserName : {0}", userName);
                return null;
            }
        }

        public IUser GetUserByEmail(string email)
        {
            try
            {
                return _userRepository.GetByEmail(email);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get user by email with Email: {0}", email);
                return null;
            }
        }

        public IUser GetUser(Guid userID)
        {
            try
            {
                return (IUser)_userRepository.Get(userID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get user with userID: {0}", userID);
                return null;
            }
        }


        public ICollection<IUser> GetAllUserPaged(int pageNumber, int pageSize, Func<IUser, dynamic> orderBy)
        {
            if (pageNumber < 0 || pageSize < 0)
                throw new ArgumentException("Invalid pageNumber and pageSize");

            try
            {
                List<IUser> result = _userRepository.GetAllPaged(pageNumber, pageSize, orderBy).Cast<IUser>().ToList<IUser>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to Get Users with values: pageNumber={0}, pageSize={1}, sort={2}",
                    pageNumber, pageSize, JsonConvert.SerializeObject(orderBy));
                return null;
            }
        }
        public bool DeleteUser(Guid userID)
        {
            try
            {
                _userRepository.Remove(userID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to Delete User with userID: {0}", userID);
                return false;
            }
        }
        public bool UpdateUserInformation(IUser user)
        {
            if (user == null)
                return false;
            try
            {
                _userRepository.Update(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to UpdateUserInformation with values: user={0}", JsonConvert.SerializeObject(user));
                return false;
            }
        }

        public ICollection<IUser> GetAll()
        {
            try
            {
                List<IUser> result = _userRepository.GetAll().Cast<IUser>().ToList<IUser>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to Get All Users");
                return new List<IUser>();
            }
        }
    }
}
