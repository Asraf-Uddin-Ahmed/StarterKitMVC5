using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Enums;

namespace $safeprojectname$.Models.Response
{
    public class UserResponseModel : ResponseModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public UserType TypeOfUser { get; set; }
        public UserStatus Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public int WrongPasswordAttempt { get; set; }
        public DateTime? LastWrongPasswordAttempt { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}