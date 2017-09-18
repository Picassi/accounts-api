namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUserFlowTypeFromFlow : DbMigration
    {
        public override void Up()
        {
            DropColumn("accounts.ScheduledTransactions", "Type");
        }
        
        public override void Down()
        {
            AddColumn("accounts.ScheduledTransactions", "Type", c => c.Int(nullable: false));
        }
    }
}
