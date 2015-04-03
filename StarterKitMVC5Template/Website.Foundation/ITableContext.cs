using System;
using System.Data.Entity;
using $safeprojectname$.Aggregates;
namespace $safeprojectname$
{
    public interface ITableContext
    {
        DbSet<Settings> Settings { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserVerification> UserVerifications { get; set; }
        DbSet<PasswordVerification> PasswordVerifications { get; set; }
    }
}
