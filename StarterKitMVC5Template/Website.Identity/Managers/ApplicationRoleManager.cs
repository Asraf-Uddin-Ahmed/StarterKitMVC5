using Website.Foundation.Core.Aggregates.Identity;
using Website.Foundation.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Managers
{
    public class ApplicationRoleManager : RoleManager<CustomRole, Guid>
    {
        public ApplicationRoleManager(IRoleStore<CustomRole, Guid> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<CustomRole, Guid, CustomUserRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }
}