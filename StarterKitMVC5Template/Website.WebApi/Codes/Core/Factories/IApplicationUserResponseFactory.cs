using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Foundation.Core.Aggregates;
using $safeprojectname$.Models.Response;

namespace $safeprojectname$.Codes.Core.Factories
{
    public interface IApplicationUserResponseFactory
    {
        ApplicationUserResponseModel Create(ApplicationUser applicationUser);
        IEnumerable<ApplicationUserResponseModel> Create(IEnumerable<ApplicationUser> applicationUsers);
    }
}
