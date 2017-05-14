using System.Data.Entity.Migrations;
using Picassi.Core.Accounts.DAL.Migrations;

namespace Picassi.Core.Accounts.DAL
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