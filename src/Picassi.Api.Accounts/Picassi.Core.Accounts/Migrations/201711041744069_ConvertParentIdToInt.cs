namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertParentIdToInt : DbMigration
    {
        public override void Up()
        {
            DropIndex("accounts.Categories", new[] { "Parent_Id" });
            DropColumn("accounts.Categories", "ParentId");
            RenameColumn(table: "accounts.Categories", name: "Parent_Id", newName: "ParentId");
            AlterColumn("accounts.Categories", "ParentId", c => c.Int());
            CreateIndex("accounts.Categories", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("accounts.Categories", new[] { "ParentId" });
            AlterColumn("accounts.Categories", "ParentId", c => c.String());
            RenameColumn(table: "accounts.Categories", name: "ParentId", newName: "Parent_Id");
            AddColumn("accounts.Categories", "ParentId", c => c.String());
            CreateIndex("accounts.Categories", "Parent_Id");
        }
    }
}
