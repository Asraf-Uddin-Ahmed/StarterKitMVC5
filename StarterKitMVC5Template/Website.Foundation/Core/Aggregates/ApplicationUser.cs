using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace $safeprojectname$.Core.Aggregates
{
    public class ApplicationUser : IdentityUser
    {
        //[Required]
        //public Guid CreatedByUserID { get; set; }
        //[Required]
        //public Guid UpdatedByUserID { get; set; }
        //[Required]
        //public DateTime CreationTime { get; set; }
        //[Required]
        //public DateTime UpdateTime { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}