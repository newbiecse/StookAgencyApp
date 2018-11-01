using System.Data.Entity.Migrations;

namespace StookAgencyApp
{
    public sealed class AppDbMigrationConfiguration : DbMigrationsConfiguration<AppDbContext>
    {
        public AppDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}