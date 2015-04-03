using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Aggregates;
using $safeprojectname$.Enums;

namespace $safeprojectname$.Repositories
{
    public interface ISettingsRepository : IBaseEfRepository<Settings>
    {
        string GetValueByName(SettingsName name);
        string GetDisplayNameByName(SettingsName name);
    }
}
