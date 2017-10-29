namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesAndTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "accounts.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Headline = c.String(),
                        Text = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        DueDate = c.DateTime(),
                        Ordinal = c.Int(nullable: false),
                        Priorty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("accounts.Tasks");
            DropTable("accounts.Notes");
        }
    }
}
