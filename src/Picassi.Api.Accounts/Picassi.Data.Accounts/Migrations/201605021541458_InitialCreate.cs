namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "accounts.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.ReportGroupCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ReportGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("accounts.ReportGroups", t => t.ReportGroup_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.ReportGroup_Id);
            
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
                "accounts.ReportGroupReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        ReportId = c.Int(nullable: false),
                        ReportGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Reports", t => t.ReportId, cascadeDelete: true)
                .ForeignKey("accounts.ReportGroups", t => t.ReportGroup_Id)
                .Index(t => t.ReportId)
                .Index(t => t.ReportGroup_Id);
            
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
                "accounts.Snapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "accounts.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        FromId = c.Int(),
                        ToId = c.Int(),
                        CategoryId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .ForeignKey("accounts.Accounts", t => t.FromId)
                .ForeignKey("accounts.Accounts", t => t.ToId)
                .Index(t => t.FromId)
                .Index(t => t.ToId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("accounts.Transactions", "ToId", "accounts.Accounts");
            DropForeignKey("accounts.Transactions", "FromId", "accounts.Accounts");
            DropForeignKey("accounts.Transactions", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.Snapshots", "AccountId", "accounts.Accounts");
            DropForeignKey("accounts.ReportGroupReports", "ReportGroup_Id", "accounts.ReportGroups");
            DropForeignKey("accounts.ReportGroupReports", "ReportId", "accounts.Reports");
            DropForeignKey("accounts.ReportGroupCategories", "ReportGroup_Id", "accounts.ReportGroups");
            DropForeignKey("accounts.ReportGroupCategories", "CategoryId", "accounts.Categories");
            DropIndex("accounts.Transactions", new[] { "CategoryId" });
            DropIndex("accounts.Transactions", new[] { "ToId" });
            DropIndex("accounts.Transactions", new[] { "FromId" });
            DropIndex("accounts.Snapshots", new[] { "AccountId" });
            DropIndex("accounts.ReportGroupReports", new[] { "ReportGroup_Id" });
            DropIndex("accounts.ReportGroupReports", new[] { "ReportId" });
            DropIndex("accounts.ReportGroupCategories", new[] { "ReportGroup_Id" });
            DropIndex("accounts.ReportGroupCategories", new[] { "CategoryId" });
            DropTable("accounts.Transactions");
            DropTable("accounts.Tags");
            DropTable("accounts.Snapshots");
            DropTable("accounts.Reports");
            DropTable("accounts.ReportGroupReports");
            DropTable("accounts.ReportGroups");
            DropTable("accounts.ReportGroupCategories");
            DropTable("accounts.Categories");
            DropTable("accounts.Accounts");
        }
    }
}
