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
                "accounts.Transactions",
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
            DropIndex("accounts.Transactions", new[] { "CategoryId" });
            DropIndex("accounts.Transactions", new[] { "ToId" });
            DropIndex("accounts.Transactions", new[] { "FromId" });
            DropIndex("accounts.Snapshots", new[] { "AccountId" });
            DropTable("accounts.Transactions");
            DropTable("accounts.Snapshots");
            DropTable("accounts.Categories");
            DropTable("accounts.Accounts");
        }
    }
}
