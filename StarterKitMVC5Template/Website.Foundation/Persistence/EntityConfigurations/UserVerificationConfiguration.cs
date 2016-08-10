using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence.EntityConfigurations
{
    public class UserVerificationConfiguration : EntityTypeConfiguration<UserVerification>
    {
        public UserVerificationConfiguration()
        {
            Property(u => u.CreationTime)
                .IsRequired();

            Property(u => u.VerificationCode)
                .IsRequired();

        }
    }
}
