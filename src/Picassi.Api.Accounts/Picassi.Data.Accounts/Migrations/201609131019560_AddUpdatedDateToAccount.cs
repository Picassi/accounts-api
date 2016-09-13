namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdatedDateToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Accounts", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("accounts.Accounts", "LastUpdated");
        }
    }
}
