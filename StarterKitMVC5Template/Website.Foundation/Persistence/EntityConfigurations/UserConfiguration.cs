using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.CreationTime)
                .IsRequired();

            Property(u => u.EncryptedPassword)
                .IsRequired()
                .HasColumnName("Password");

            Property(u => u.EmailAddress)
                .IsRequired()
                .HasMaxLength(255);

            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(u => u.Status)
                .IsRequired();

            Property(u => u.TypeOfUser)
                .IsRequired();

            Property(u => u.UpdateTime)
                .IsRequired();

            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(255);

            Property(u => u.WrongPasswordAttempt)
                .IsRequired();


            HasMany(u => u.PasswordVerifications)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UserID);

            HasMany(u => u.UserVerifications)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UserID);
           
        }
    }
}
