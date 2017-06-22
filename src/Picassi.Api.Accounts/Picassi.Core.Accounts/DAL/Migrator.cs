using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Microsoft.Owin;
using Picassi.Core.Accounts.Migrations;

namespace Picassi.Core.Accounts.DAL
{
    public interface IMigrator
    {
        void ApplyPendingMigrations();
    }

    public class Migrator : IMigrator
    {
        private readonly IOwinContext _context;

        public Migrator(IOwinContext context)
        {
            _context = context;
        }

        public void ApplyPendingMigrations()
        {
            var configuration = new Configuration();
            configuration.TargetDatabase = new DbConnectionInfo(AccountsDatabaseProvider.BuildConnectionString(_context), "System.Data.SqlClient");
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}