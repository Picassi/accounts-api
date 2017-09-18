namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultAccountSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Budgets", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("accounts.Budgets", "IsDefault");
        }
    }
}
