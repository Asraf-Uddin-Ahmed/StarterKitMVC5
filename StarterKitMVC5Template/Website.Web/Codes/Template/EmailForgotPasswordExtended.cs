using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Foundation.Aggregates;

namespace $safeprojectname$.Codes.Template
{
    public partial class EmailForgotPassword
    {
        public IUser RegisteredUser
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public EmailForgotPassword(IUser registeredUser, string url)
        {
            RegisteredUser = registeredUser;
            Url = url;
        }
    }
}