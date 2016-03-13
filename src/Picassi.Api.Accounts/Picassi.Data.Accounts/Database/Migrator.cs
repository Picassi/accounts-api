using System.Data.Entity.Migrations;
using Picassi.Data.Accounts.Migrations;

namespace Picassi.Data.Accounts.Database
{
    public class Migrator
    {
        public static void ApplyPendingMigrations()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}