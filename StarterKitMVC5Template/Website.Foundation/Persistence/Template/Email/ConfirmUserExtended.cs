using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence.Template.Email
{
    public partial class ConfirmUser
    {
        public User NewUser
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public ConfirmUser(User newUser, string url)
        {
            NewUser = newUser;
            Url = url;
        }
    }
}