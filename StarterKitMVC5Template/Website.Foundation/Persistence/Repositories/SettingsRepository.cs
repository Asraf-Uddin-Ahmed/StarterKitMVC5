using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Enums;

namespace $safeprojectname$.Persistence.Repositories
{
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        private ApplicationDbContext _context;
        public SettingsRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        // For one time loading make it 'static'
        private List<Settings> _listAllSettings;
        private List<Settings> _ListAllSettings
        {
            get
            {
                if(_listAllSettings == null)
                {
                    try
                    {
                        _listAllSettings = _context.Settings.ToList<Settings>();
                    }
                    catch(Exception)
                    {
                        _listAllSettings = new List<Settings>();
                    }
                }
                return _listAllSettings;
            }
        }

        public string GetValueByName(SettingsName name)
        {
            Settings settings = _ListAllSettings.Where(col => col.Name == name.ToString()).FirstOrDefault();
            string value = settings == null ? null : settings.Value;
            return value;
        }
        public string GetDisplayNameByName(SettingsName name)
        {
            Settings settings = _ListAllSettings.Where(col => col.Name == name.ToString()).FirstOrDefault();
            string displayName = settings == null ? null : settings.DisplayName;
            return displayName;
        }
    }
}
