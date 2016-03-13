namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Snapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        FromId = c.Int(),
                        ToId = c.Int(),
                        CategoryId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Accounts", t => t.FromId)
                .ForeignKey("dbo.Accounts", t => t.ToId)
                .Index(t => t.FromId)
                .Index(t => t.ToId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "ToId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "FromId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Snapshots", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            DropIndex("dbo.Transactions", new[] { "ToId" });
            DropIndex("dbo.Transactions", new[] { "FromId" });
            DropIndex("dbo.Snapshots", new[] { "AccountId" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Snapshots");
            DropTable("dbo.Categories");
            DropTable("dbo.Accounts");
        }
    }
}
