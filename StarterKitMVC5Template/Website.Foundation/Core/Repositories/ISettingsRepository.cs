using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;
using $safeprojectname$.Core.Repositories;

namespace $safeprojectname$.Core.Repositories
{
    public interface ISettingsRepository : IRepository<Settings>
    {
        string GetValueByName(SettingsName name);
        string GetDisplayNameByName(SettingsName name);
    }
}
