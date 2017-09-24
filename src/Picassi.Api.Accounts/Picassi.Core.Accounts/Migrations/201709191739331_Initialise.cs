namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialise : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "accounts.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        Goal_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Goals", t => t.Goal_Id)
                .Index(t => t.Goal_Id);
            
            CreateTable(
                "accounts.Budgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Period = c.Int(nullable: false),
                        AggregationPeriod = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("accounts.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "accounts.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "accounts.ModelledTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        Description = c.String(),
                        AccountId = c.Int(nullable: false),
                        ToId = c.Int(),
                        CategoryId = c.Int(),
                        EventId = c.Int(),
                        BudgetId = c.Int(),
                        ScheduledTransactionId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Tag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("accounts.Budgets", t => t.BudgetId)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .ForeignKey("accounts.Events", t => t.EventId)
                .ForeignKey("accounts.ScheduledTransactions", t => t.ScheduledTransactionId)
                .ForeignKey("accounts.Accounts", t => t.ToId)
                .ForeignKey("accounts.Tags", t => t.Tag_Id)
                .Index(t => t.AccountId)
                .Index(t => t.ToId)
                .Index(t => t.CategoryId)
                .Index(t => t.EventId)
                .Index(t => t.BudgetId)
                .Index(t => t.ScheduledTransactionId)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "accounts.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "accounts.ScheduledTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        AccountId = c.Int(nullable: false),
                        ToId = c.Int(),
                        CategoryId = c.Int(),
                        EventId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        DaysBefore = c.Int(),
                        Recurrence = c.Int(),
                        RecurrenceDayOfMonth = c.Int(),
                        RecurrenceWeekOfMonth = c.Int(),
                        RecurrenceDayOfWeek = c.Int(),
                        Tag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .ForeignKey("accounts.Events", t => t.EventId)
                .ForeignKey("accounts.Accounts", t => t.ToId)
                .ForeignKey("accounts.Tags", t => t.Tag_Id)
                .Index(t => t.AccountId)
                .Index(t => t.ToId)
                .Index(t => t.CategoryId)
                .Index(t => t.EventId)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "accounts.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        Description = c.String(),
                        AccountId = c.Int(nullable: false),
                        ToId = c.Int(),
                        CategoryId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        ScheduledTransactionId = c.Int(),
                        Tag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("accounts.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("accounts.Categories", t => t.CategoryId)
                .ForeignKey("accounts.ScheduledTransactions", t => t.ScheduledTransactionId)
                .ForeignKey("accounts.Accounts", t => t.ToId)
                .ForeignKey("accounts.Tags", t => t.Tag_Id)
                .Index(t => t.AccountId)
                .Index(t => t.ToId)
                .Index(t => t.CategoryId)
                .Index(t => t.ScheduledTransactionId)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "accounts.Goals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false),
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
            DropForeignKey("accounts.Transactions", "Tag_Id", "accounts.Tags");
            DropForeignKey("accounts.ScheduledTransactions", "Tag_Id", "accounts.Tags");
            DropForeignKey("accounts.ModelledTransactions", "Tag_Id", "accounts.Tags");
            DropForeignKey("accounts.Accounts", "Goal_Id", "accounts.Goals");
            DropForeignKey("accounts.Transactions", "ToId", "accounts.Accounts");
            DropForeignKey("accounts.Transactions", "ScheduledTransactionId", "accounts.ScheduledTransactions");
            DropForeignKey("accounts.Transactions", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.Transactions", "AccountId", "accounts.Accounts");
            DropForeignKey("accounts.ModelledTransactions", "ToId", "accounts.Accounts");
            DropForeignKey("accounts.ModelledTransactions", "ScheduledTransactionId", "accounts.ScheduledTransactions");
            DropForeignKey("accounts.ScheduledTransactions", "ToId", "accounts.Accounts");
            DropForeignKey("accounts.ScheduledTransactions", "EventId", "accounts.Events");
            DropForeignKey("accounts.ScheduledTransactions", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.ScheduledTransactions", "AccountId", "accounts.Accounts");
            DropForeignKey("accounts.ModelledTransactions", "EventId", "accounts.Events");
            DropForeignKey("accounts.Events", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.ModelledTransactions", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.ModelledTransactions", "BudgetId", "accounts.Budgets");
            DropForeignKey("accounts.ModelledTransactions", "AccountId", "accounts.Accounts");
            DropForeignKey("accounts.Budgets", "CategoryId", "accounts.Categories");
            DropForeignKey("accounts.Budgets", "AccountId", "accounts.Accounts");
            DropIndex("accounts.Transactions", new[] { "Tag_Id" });
            DropIndex("accounts.Transactions", new[] { "ScheduledTransactionId" });
            DropIndex("accounts.Transactions", new[] { "CategoryId" });
            DropIndex("accounts.Transactions", new[] { "ToId" });
            DropIndex("accounts.Transactions", new[] { "AccountId" });
            DropIndex("accounts.ScheduledTransactions", new[] { "Tag_Id" });
            DropIndex("accounts.ScheduledTransactions", new[] { "EventId" });
            DropIndex("accounts.ScheduledTransactions", new[] { "CategoryId" });
            DropIndex("accounts.ScheduledTransactions", new[] { "ToId" });
            DropIndex("accounts.ScheduledTransactions", new[] { "AccountId" });
            DropIndex("accounts.Events", new[] { "CategoryId" });
            DropIndex("accounts.ModelledTransactions", new[] { "Tag_Id" });
            DropIndex("accounts.ModelledTransactions", new[] { "ScheduledTransactionId" });
            DropIndex("accounts.ModelledTransactions", new[] { "BudgetId" });
            DropIndex("accounts.ModelledTransactions", new[] { "EventId" });
            DropIndex("accounts.ModelledTransactions", new[] { "CategoryId" });
            DropIndex("accounts.ModelledTransactions", new[] { "ToId" });
            DropIndex("accounts.ModelledTransactions", new[] { "AccountId" });
            DropIndex("accounts.Budgets", new[] { "AccountId" });
            DropIndex("accounts.Budgets", new[] { "CategoryId" });
            DropIndex("accounts.Accounts", new[] { "Goal_Id" });
            DropTable("accounts.Tags");
            DropTable("accounts.Goals");
            DropTable("accounts.Transactions");
            DropTable("accounts.ScheduledTransactions");
            DropTable("accounts.Events");
            DropTable("accounts.ModelledTransactions");
            DropTable("accounts.Categories");
            DropTable("accounts.Budgets");
            DropTable("accounts.Accounts");
        }
    }
}
