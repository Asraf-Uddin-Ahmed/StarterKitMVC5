using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Core.Aggregates
{
    public class PasswordVerification : Entity
    {
        public Guid UserID { get; set; }
        public User User { get; set; }

        public string VerificationCode { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
