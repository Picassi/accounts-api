using System.Data.Entity.Migrations;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Core.Accounts.Migrations
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
