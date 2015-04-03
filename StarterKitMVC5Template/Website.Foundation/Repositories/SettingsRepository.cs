using $safeprojectname$.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using $safeprojectname$.Enums;

namespace $safeprojectname$.Repositories
{
    public class SettingsRepository : BaseEfRepository<Settings>, ISettingsRepository
    {
        private TableContext _context;
        [Inject]
        public SettingsRepository(TableContext context) : base(context)
        {
            _context = context;
        }

        private List<ISettings> _listAllSettings;
        private List<ISettings> _ListAllSettings
        {
            get
            {
                if(_listAllSettings == null)
                {
                    try
                    {
                        _listAllSettings = _context.Settings.ToList<ISettings>();
                    }
                    catch(Exception)
                    {
                        _listAllSettings = new List<ISettings>();
                    }
                }
                return _listAllSettings;
            }
        }

        public string GetValueByName(SettingsName name)
        {
            ISettings settings = _ListAllSettings.Where(col => col.Name == name.ToString()).FirstOrDefault();
            string value = settings == null ? null : settings.Value;
            return value;
        }
        public string GetDisplayNameByName(SettingsName name)
        {
            ISettings settings = _ListAllSettings.Where(col => col.Name == name.ToString()).FirstOrDefault();
            string displayName = settings == null ? null : settings.DisplayName;
            return displayName;
        }
    }
}
