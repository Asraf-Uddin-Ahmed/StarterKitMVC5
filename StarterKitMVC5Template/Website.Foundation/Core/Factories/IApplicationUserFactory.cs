using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;
using $safeprojectname$.Core.Aggregates.Identity;

namespace $safeprojectname$.Core.Factories
{
    public interface IApplicationUserFactory
    {
        ApplicationUser Create(string userName, string email);
    }
}
