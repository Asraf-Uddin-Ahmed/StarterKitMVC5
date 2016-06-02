using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence.Template
{
    public partial class EmailConfirmUser
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

        public EmailConfirmUser(User newUser, string url)
        {
            NewUser = newUser;
            Url = url;
        }
    }
}