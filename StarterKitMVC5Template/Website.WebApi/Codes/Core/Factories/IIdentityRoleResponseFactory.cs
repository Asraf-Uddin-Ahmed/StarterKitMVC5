using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Models.Response;

namespace $safeprojectname$.Codes.Core.Factories
{
    public interface IIdentityRoleResponseFactory
    {
        IdentityRoleResponseModel Create(IdentityRole appRole);
        IEnumerable<IdentityRoleResponseModel> Create(IEnumerable<IdentityRole> identityRoles);
    }
}
