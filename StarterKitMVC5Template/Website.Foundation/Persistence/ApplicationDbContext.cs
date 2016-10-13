using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Aggregates.Identity;
using $safeprojectname$.Persistence.EntityConfigurations;

namespace $safeprojectname$.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public DbSet<Settings> Settings { get; set; }


        public DbSet<User> ExtendedUsers { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }
        public DbSet<PasswordVerification> PasswordVerifications { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PasswordVerificationConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserVerificationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}