using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Microsoft.Owin;
using Picassi.Core.Accounts.Migrations;

namespace Picassi.Core.Accounts.DAL
{
    public interface IMigrator
    {
        void ApplyPendingMigrations();
        void ApplyPendingMigrations(string databaseName);
    }

    public class Migrator : IMigrator
    {
        private readonly IOwinContext _context;
        public const string DatabaseProvider = "System.Data.SqlClient";

        public Migrator(IOwinContext context)
        {
            _context = context;
        }

        public void ApplyPendingMigrations()
        {
            var configuration = new Configuration
            {
                TargetDatabase = new DbConnectionInfo(AccountsDatabaseProvider.BuildConnectionString(_context), DatabaseProvider)
            };
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        public void ApplyPendingMigrations(string databaseName)
        {
            var configuration = new Configuration
            {
                TargetDatabase = new DbConnectionInfo(AccountsDatabaseProvider.BuildConnectionString(databaseName), DatabaseProvider)
            };
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}