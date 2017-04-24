using Picassi.Data.Accounts.Database.StoreProcedures;

namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionToolsStoredProc : DbMigration
    {
        public override void Up()
        {
            Sql(new GetTransactionTotals().GetStoredProcedure());
        }
        
        public override void Down()
        {
        }        
    }
}
