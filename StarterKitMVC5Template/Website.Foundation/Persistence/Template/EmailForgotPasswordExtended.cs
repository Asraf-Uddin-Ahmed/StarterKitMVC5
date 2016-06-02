using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence.Template
{
    public partial class EmailForgotPassword
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

        public EmailForgotPassword(User registeredUser, string url)
        {
            RegisteredUser = registeredUser;
            Url = url;
        }
    }
}