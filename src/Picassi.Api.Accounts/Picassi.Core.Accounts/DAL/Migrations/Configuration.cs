using System.Data.Entity.Migrations;

namespace Picassi.Core.Accounts.DAL.Migrations
{
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
