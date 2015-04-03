using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Enums;

namespace $safeprojectname$.Aggregates
{
    public class Settings : Entity, ISettings
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public SettingsType Type { get; set; }

    }
}
