using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Enums;

namespace $safeprojectname$.Core.Aggregates
{
    public class User : Entity
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [Column("Password")]
        public string EncryptedPassword { get; set; }

        [Required]
        public string EmailAddress { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public UserType TypeOfUser { get; set; }

        [Required]
        public UserStatus Status { get; set; }
        
        public DateTime? LastLogin { get; set; }

        [Required]
        public int WrongPasswordAttempt { get; set; }

        public DateTime? LastWrongPasswordAttempt { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public DateTime UpdateTime { get; set; }


        public virtual ICollection<UserVerification> UserVerifications { get; set; }
        public virtual ICollection<PasswordVerification> PasswordVerifications { get; set; }


        private string _decryptedPassword;
        public string DecryptedPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_decryptedPassword))
                    _decryptedPassword = CryptographicUtility.Decrypt(this.EncryptedPassword, this.ID); ;
                return _decryptedPassword;
            }
        }
    }
}
