using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Foundation.Aggregates;

namespace $safeprojectname$.Codes.Template
{
    public partial class EmailConfirmUser
    {
        public IUser NewUser
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public EmailConfirmUser(IUser newUser, string url)
        {
            NewUser = newUser;
            Url = url;
        }
    }
}