namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatementTransactionNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Transactions", "StatementTransactionNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("accounts.Transactions", "StatementTransactionNumber");
        }
    }
}
