namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Tasks", "Completed", c => c.Boolean(nullable: false));
            AddColumn("accounts.Tasks", "CompletedDate", c => c.DateTime());
            AddColumn("accounts.Tasks", "Priority", c => c.Int(nullable: false));
            DropColumn("accounts.Tasks", "Priorty");
        }
        
        public override void Down()
        {
            AddColumn("accounts.Tasks", "Priorty", c => c.Int(nullable: false));
            DropColumn("accounts.Tasks", "Priority");
            DropColumn("accounts.Tasks", "CompletedDate");
            DropColumn("accounts.Tasks", "Completed");
        }
    }
}
