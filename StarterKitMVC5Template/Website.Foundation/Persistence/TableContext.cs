using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Persistence
{
    public class TableContext : DbContext
    {
        public TableContext()
            : base("name=DbConnectionString")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<PasswordVerification> PasswordVerifications { get; set; }

    }
}