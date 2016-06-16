using Newtonsoft.Json;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.SearchData;
using $safeprojectname$.Core.Services;

namespace $safeprojectname$.Persistence.Services
{
    public class UserService : IUserService
    {
        private ILogger _logger;
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IPasswordVerificationRepository _passwordVerificationRepository;
        [Inject]
        public UserService(ILogger logger,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordVerificationRepository passwordVerificationRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordVerificationRepository = passwordVerificationRepository;
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

        public User GetUserByUserName(string userName)
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

        public User GetUserByEmail(string email)
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

        public User GetUser(Guid userID)
        {
            try
            {
                return _userRepository.Get(userID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get user with userID: {0}", userID);
                return null;
            }
        }


        public ICollection<User> GetUserBy(Pagination pagination, OrderBy<User> sortBy)
        {
            try
            {
                List<User> result = _userRepository.GetBy(pagination, sortBy).Cast<User>().ToList<User>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to Get Users with values: pagination={0}, sort={1}",
                    JsonConvert.SerializeObject(pagination), JsonConvert.SerializeObject(sortBy));
                return null;
            }
        }
        public bool DeleteUser(Guid userID)
        {
            try
            {
                _userRepository.Remove(userID);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to Delete User with userID: {0}", userID);
                return false;
            }
        }
        public bool UpdateUserInformation(User user)
        {
            if (user == null)
                return false;
            try
            {
                // You can also give the 'isPersist' value 'true' for instant save.
                // See 'DeleteUser' for detail.
                _userRepository.Update(user);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to UpdateUserInformation with values: user={0}", JsonConvert.SerializeObject(user));
                return false;
            }
        }

        public ICollection<User> GetAll()
        {
            try
            {
                List<User> result = _userRepository.GetAll().ToList<User>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to Get All Users");
                return new List<User>();
            }
        }

        public User GetUserByPasswordVerificationCode(string verificationCode)
        {
            PasswordVerification passwordVerification = _passwordVerificationRepository.GetByVerificationCode(verificationCode);
            return passwordVerification == null ? null : this.GetUser(passwordVerification.UserID);
        }

        public int GetTotal()
        {
            return _userRepository.GetTotal();
        }
    }
}
