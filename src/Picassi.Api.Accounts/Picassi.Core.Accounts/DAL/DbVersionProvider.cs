using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Configuration = Picassi.Core.Accounts.Migrations.Configuration;

namespace Picassi.Core.Accounts.DAL
{
    public interface IDbVersionProvider
    {
        string GetLatestVersion();
        string GetVersionForActiveUser();
        string GetVersionForDatabase(string taskDatabaseName);
    }

    public class DbVersionProvider : IDbVersionProvider
    {
        private readonly IOwinContext _owinContext;

        public DbVersionProvider(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public string GetLatestVersion()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            return CompileVersionNumber(migrator.GetLocalMigrations().ToList().Count);
        }

        public string GetVersionForActiveUser()
        {
            var configuration = new Configuration
            {
                TargetDatabase = new DbConnectionInfo(AccountsDatabaseProvider.BuildConnectionString(_owinContext), Migrator.DatabaseProvider)
            };
            var migrator = new DbMigrator(configuration);
            return CompileVersionNumber(migrator.GetDatabaseMigrations().ToList().Count);
        }

        public string GetVersionForDatabase(string taskDatabaseName)
        {
            var configuration = new Configuration
            {
                TargetDatabase = new DbConnectionInfo(AccountsDatabaseProvider.BuildConnectionString(taskDatabaseName), Migrator.DatabaseProvider)
            };
            var migrator = new DbMigrator(configuration);
            return CompileVersionNumber(migrator.GetDatabaseMigrations().ToList().Count);
        }

        private string CompileVersionNumber(int migrationCount)
        {
            var majorVersion = ConfigurationManager.AppSettings["major-version"];
            var minorVersion = ConfigurationManager.AppSettings["minor-version"];
            var revisionVersion = ConfigurationManager.AppSettings["revision"];
            return $"{majorVersion}.{minorVersion}.{revisionVersion}.{migrationCount}";
        }
    }
}