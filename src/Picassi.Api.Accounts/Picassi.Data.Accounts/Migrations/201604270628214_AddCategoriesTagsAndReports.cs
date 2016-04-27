namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriesTagsAndReports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "accounts.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.ReportLineDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Ordinal = c.Int(nullable: false),
                        ReportId = c.Int(nullable: false),
                        GroupedId = c.Int(),
                        CategoryId = c.Int(),
                        TagId = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .ForeignKey("accounts.Groups", t => t.Group_Id)
                .ForeignKey("accounts.Reports", t => t.ReportId, cascadeDelete: true)
                .ForeignKey("accounts.Tags", t => t.TagId)
                .Index(t => t.ReportId)
                .Index(t => t.CategoryId)
                .Index(t => t.TagId)
                .Index(t => t.Group_Id);
            
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
                "accounts.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("accounts.ReportLineDefinitions", "TagId", "accounts.Tags");
            DropForeignKey("accounts.ReportLineDefinitions", "ReportId", "accounts.Reports");
            DropForeignKey("accounts.ReportLineDefinitions", "Group_Id", "accounts.Groups");
            DropForeignKey("accounts.ReportLineDefinitions", "CategoryId", "accounts.Categories");
            DropIndex("accounts.ReportLineDefinitions", new[] { "Group_Id" });
            DropIndex("accounts.ReportLineDefinitions", new[] { "TagId" });
            DropIndex("accounts.ReportLineDefinitions", new[] { "CategoryId" });
            DropIndex("accounts.ReportLineDefinitions", new[] { "ReportId" });
            DropTable("accounts.Tags");
            DropTable("accounts.Reports");
            DropTable("accounts.ReportLineDefinitions");
            DropTable("accounts.Groups");
        }
    }
}
