using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Services;
using $safeprojectname$.App_Start;

namespace $safeprojectname$.Models.Account
{
    public class ShowUserModel
    {
        private IUserService _userService;

        private List<User> _listUser;
        public List<User> ListUser
        {
            get
            {
                if(_listUser == null)
                {
                    _listUser = _userService.GetAll().ToList();
                }
                return _listUser;
            }
        }

        public ShowUserModel()
        {
            _userService = NinjectWebCommon.GetConcreteInstance<IUserService>();
        }
    }
}