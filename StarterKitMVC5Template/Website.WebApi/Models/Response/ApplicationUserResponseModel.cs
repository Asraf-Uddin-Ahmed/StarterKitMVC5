using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using Website.Foundation.Core.Aggregates;

namespace $safeprojectname$.Models.Response
{
    public class ApplicationUserResponseModel : ResponseModel
    {
        public string RoleUrl { get; set; }
        public string ClaimUrl { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        
    }
}