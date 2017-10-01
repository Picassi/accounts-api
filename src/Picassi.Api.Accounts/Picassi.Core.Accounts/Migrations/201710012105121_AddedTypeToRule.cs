namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTypeToRule : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.AssignmentRules", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("accounts.AssignmentRules", "Type");
        }
    }
}
