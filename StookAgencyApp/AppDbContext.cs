using Microsoft.AspNet.Identity.EntityFramework;
using StookAgencyApp.Models;
using System.Data.Entity;

namespace StookAgencyApp
{
    public class AppDbContext : IdentityDbContext<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public IDbSet<Customer> Customers { get; set; }

        public AppDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<AppDbContext>(new AppDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users", "dbo");
            modelBuilder.Entity<Role>().ToTable("Roles", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");
        }
    }
}