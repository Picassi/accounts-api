namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAdditionalIdFromReportGroupReport : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE accounts.ReportGroupReports SET ReportGroup_Id = GroupId");
            DropForeignKey("accounts.ReportGroupReports", "ReportGroup_Id", "accounts.ReportGroups");
            DropIndex("accounts.ReportGroupReports", new[] { "ReportGroup_Id" });
            RenameColumn(table: "accounts.ReportGroupReports", name: "ReportGroup_Id", newName: "ReportGroupId");
            AlterColumn("accounts.ReportGroupReports", "ReportGroupId", c => c.Int(nullable: false));
            CreateIndex("accounts.ReportGroupReports", "ReportGroupId");
            AddForeignKey("accounts.ReportGroupReports", "ReportGroupId", "accounts.ReportGroups", "Id", cascadeDelete: true);
            DropColumn("accounts.ReportGroupReports", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("accounts.ReportGroupReports", "GroupId", c => c.Int(nullable: false));
            DropForeignKey("accounts.ReportGroupReports", "ReportGroupId", "accounts.ReportGroups");
            DropIndex("accounts.ReportGroupReports", new[] { "ReportGroupId" });
            AlterColumn("accounts.ReportGroupReports", "ReportGroupId", c => c.Int());
            RenameColumn(table: "accounts.ReportGroupReports", name: "ReportGroupId", newName: "ReportGroup_Id");
            CreateIndex("accounts.ReportGroupReports", "ReportGroup_Id");
            AddForeignKey("accounts.ReportGroupReports", "ReportGroup_Id", "accounts.ReportGroups", "Id");
        }
    }
}
