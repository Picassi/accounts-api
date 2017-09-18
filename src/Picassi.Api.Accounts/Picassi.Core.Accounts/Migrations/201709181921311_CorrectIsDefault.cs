namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectIsDefault : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Accounts", "IsDefault", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn("accounts.Budgets", "IsDefault");
        }
        
        public override void Down()
        {
            AddColumn("accounts.Budgets", "IsDefault", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn("accounts.Accounts", "IsDefault");
        }
    }
}
