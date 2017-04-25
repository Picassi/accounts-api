namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBloatFromDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("accounts.ReportGroupCategories", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.ReportGroupCategories", "ReportGroupId", "accounts.ReportGroups");
            DropForeignKey("accounts.ReportGroupReports", "ReportId", "accounts.Reports");
            DropForeignKey("accounts.ReportGroupReports", "ReportGroupId", "accounts.ReportGroups");
            DropForeignKey("accounts.Snapshots", "AccountId", "accounts.Accounts");
            DropIndex("accounts.ReportGroupCategories", new[] { "ReportGroupId" });
            DropIndex("accounts.ReportGroupCategories", new[] { "CategoryId" });
            DropIndex("accounts.ReportGroupReports", new[] { "ReportGroupId" });
            DropIndex("accounts.ReportGroupReports", new[] { "ReportId" });
            DropIndex("accounts.Snapshots", new[] { "AccountId" });
            DropTable("accounts.ReportGroupCategories");
            DropTable("accounts.ReportGroups");
            DropTable("accounts.ReportGroupReports");
            DropTable("accounts.Reports");
            DropTable("accounts.Snapshots");
            DropTable("accounts.Tags");
        }
        
        public override void Down()
        {
            CreateTable(
                "accounts.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.Snapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ReportType = c.Int(nullable: false),
                        ReportSource = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.ReportGroupReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        ReportGroupId = c.Int(nullable: false),
                        ReportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.ReportGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.ReportGroupCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        ReportGroupId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("accounts.Snapshots", "AccountId");
            CreateIndex("accounts.ReportGroupReports", "ReportId");
            CreateIndex("accounts.ReportGroupReports", "ReportGroupId");
            CreateIndex("accounts.ReportGroupCategories", "CategoryId");
            CreateIndex("accounts.ReportGroupCategories", "ReportGroupId");
            AddForeignKey("accounts.Snapshots", "AccountId", "accounts.Accounts", "Id", cascadeDelete: true);
            AddForeignKey("accounts.ReportGroupReports", "ReportGroupId", "accounts.ReportGroups", "Id", cascadeDelete: true);
            AddForeignKey("accounts.ReportGroupReports", "ReportId", "accounts.Reports", "Id", cascadeDelete: true);
            AddForeignKey("accounts.ReportGroupCategories", "ReportGroupId", "accounts.ReportGroups", "Id", cascadeDelete: true);
            AddForeignKey("accounts.ReportGroupCategories", "CategoryId", "accounts.Categories", "Id", cascadeDelete: true);
        }
    }
}
