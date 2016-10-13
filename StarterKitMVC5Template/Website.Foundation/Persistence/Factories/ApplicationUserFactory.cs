using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;
using $safeprojectname$.Core.Factories;
using $safeprojectname$.Core.Aggregates.Identity;

namespace $safeprojectname$.Persistence.Factories
{
    public class ApplicationUserFactory : IApplicationUserFactory
    {
        public ApplicationUser Create(string userName, string email)
        {
            ApplicationUser appUser = new ApplicationUser()
            {
                UserName = userName,
                Email = email,
                Id = GuidUtility.GetNewSequentialGuid(),
            };
            return appUser;
        }
    }
}
