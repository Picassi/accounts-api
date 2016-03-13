using Picassi.Data.Accounts.Database;

namespace Picassi.Data.Accounts.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AccountsDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AccountsDataContext context)
        {

        }
    }
}
