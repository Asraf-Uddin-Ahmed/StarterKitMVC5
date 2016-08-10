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
        public string UserName { get; set; }

        public string EncryptedPassword { get; set; }

        public string EmailAddress { get; set; }
        
        public string Name { get; set; }

        public UserType TypeOfUser { get; set; }

        public UserStatus Status { get; set; }
        
        public DateTime? LastLogin { get; set; }

        public int WrongPasswordAttempt { get; set; }

        public DateTime? LastWrongPasswordAttempt { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }


        public ICollection<UserVerification> UserVerifications { get; set; }
        public ICollection<PasswordVerification> PasswordVerifications { get; set; }


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
