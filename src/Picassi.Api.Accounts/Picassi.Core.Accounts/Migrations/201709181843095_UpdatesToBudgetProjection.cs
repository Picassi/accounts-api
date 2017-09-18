namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatesToBudgetProjection : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Budgets", "AccountId", c => c.Int(nullable: false));
            AddColumn("accounts.ModelledTransactions", "BudgetId", c => c.Int());
            CreateIndex("accounts.Budgets", "AccountId");
            CreateIndex("accounts.ModelledTransactions", "BudgetId");
            AddForeignKey("accounts.Budgets", "AccountId", "accounts.Accounts", "Id", cascadeDelete: true);
            AddForeignKey("accounts.ModelledTransactions", "BudgetId", "accounts.Budgets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("accounts.ModelledTransactions", "BudgetId", "accounts.Budgets");
            DropForeignKey("accounts.Budgets", "AccountId", "accounts.Accounts");
            DropIndex("accounts.ModelledTransactions", new[] { "BudgetId" });
            DropIndex("accounts.Budgets", new[] { "AccountId" });
            DropColumn("accounts.ModelledTransactions", "BudgetId");
            DropColumn("accounts.Budgets", "AccountId");
        }
    }
}
