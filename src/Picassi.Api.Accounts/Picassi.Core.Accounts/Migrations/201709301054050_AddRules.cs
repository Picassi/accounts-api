namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "accounts.AssignmentRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(),
                        DescriptionRegex = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        Priority = c.Int(),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            AddColumn("accounts.Accounts", "AssignmentRule_Id", c => c.Int());
            CreateIndex("accounts.Accounts", "AssignmentRule_Id");
            AddForeignKey("accounts.Accounts", "AssignmentRule_Id", "accounts.AssignmentRules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("accounts.AssignmentRules", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.Accounts", "AssignmentRule_Id", "accounts.AssignmentRules");
            DropIndex("accounts.AssignmentRules", new[] { "CategoryId" });
            DropIndex("accounts.Accounts", new[] { "AssignmentRule_Id" });
            DropColumn("accounts.Accounts", "AssignmentRule_Id");
            DropTable("accounts.AssignmentRules");
        }
    }
}
