namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParentsAndGroupsToCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Categories", "ParentId", c => c.String());
            AddColumn("accounts.Categories", "CategoryType", c => c.Int(nullable: false));
            AddColumn("accounts.Categories", "Parent_Id", c => c.Int());
            CreateIndex("accounts.Categories", "Parent_Id");
            AddForeignKey("accounts.Categories", "Parent_Id", "accounts.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("accounts.Categories", "Parent_Id", "accounts.Categories");
            DropIndex("accounts.Categories", new[] { "Parent_Id" });
            DropColumn("accounts.Categories", "Parent_Id");
            DropColumn("accounts.Categories", "CategoryType");
            DropColumn("accounts.Categories", "ParentId");
        }
    }
}
