using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence.Template.Email
{
    public partial class ForgotPassword
    {
        public User RegisteredUser
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public ForgotPassword(User registeredUser, string url)
        {
            RegisteredUser = registeredUser;
            Url = url;
        }
    }
}