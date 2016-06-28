using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Constants;

namespace $safeprojectname$.Models
{
    public class RegisterExternalBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public ExternalLoginProviderName Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }

        [Required]
        public string ClientID { get; set; }
    }
}
