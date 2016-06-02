using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;

namespace $safeprojectname$.Core.Factories
{
    public interface IUserFactory
    {
        User Create(string password);
    }
}
