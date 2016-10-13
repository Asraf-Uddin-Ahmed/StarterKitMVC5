using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Models.Request.Role
{
    public class UsersInRoleRequestModel : RequestModel
    {

        public Guid Id { get; set; }
        public List<Guid> EnrolledUsers { get; set; }
        public List<Guid> RemovedUsers { get; set; }
    }
}