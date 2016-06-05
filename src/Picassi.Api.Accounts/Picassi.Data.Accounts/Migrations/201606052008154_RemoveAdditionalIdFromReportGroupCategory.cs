namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAdditionalIdFromReportGroupCategory : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE accounts.ReportGroupCategories SET ReportGroup_Id = GroupId");
            DropForeignKey("accounts.ReportGroupCategories", "ReportGroup_Id", "accounts.ReportGroups");
            DropIndex("accounts.ReportGroupCategories", new[] { "ReportGroup_Id" });
            RenameColumn(table: "accounts.ReportGroupCategories", name: "ReportGroup_Id", newName: "ReportGroupId");
            AlterColumn("accounts.ReportGroupCategories", "ReportGroupId", c => c.Int(nullable: false));
            CreateIndex("accounts.ReportGroupCategories", "ReportGroupId");
            AddForeignKey("accounts.ReportGroupCategories", "ReportGroupId", "accounts.ReportGroups", "Id", cascadeDelete: true);
            DropColumn("accounts.ReportGroupCategories", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("accounts.ReportGroupCategories", "GroupId", c => c.Int(nullable: false));
            DropForeignKey("accounts.ReportGroupCategories", "ReportGroupId", "accounts.ReportGroups");
            DropIndex("accounts.ReportGroupCategories", new[] { "ReportGroupId" });
            AlterColumn("accounts.ReportGroupCategories", "ReportGroupId", c => c.Int());
            RenameColumn(table: "accounts.ReportGroupCategories", name: "ReportGroupId", newName: "ReportGroup_Id");
            CreateIndex("accounts.ReportGroupCategories", "ReportGroup_Id");
            AddForeignKey("accounts.ReportGroupCategories", "ReportGroup_Id", "accounts.ReportGroups", "Id");
        }
    }
}
