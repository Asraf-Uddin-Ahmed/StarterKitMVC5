using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Enums;

namespace $safeprojectname$.Aggregates
{
    public class UserVerification : Entity, IUserVerification
    {
        [Required]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }
    }
}
