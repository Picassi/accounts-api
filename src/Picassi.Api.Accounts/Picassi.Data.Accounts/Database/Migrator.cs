using System.Data.Entity.Migrations;
using Picassi.Data.Accounts.Migrations;

namespace Picassi.Data.Accounts.Database
{
    public interface IMigrator
    {
        void ApplyPendingMigrations();
    }

    public class Migrator : IMigrator
    {
        public void ApplyPendingMigrations()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}