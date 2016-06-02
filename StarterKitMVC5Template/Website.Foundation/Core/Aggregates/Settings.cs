using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Enums;

namespace $safeprojectname$.Core.Aggregates
{
    public class Settings : Entity
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
