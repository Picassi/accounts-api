namespace Picassi.Core.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkTransactionToScheduledTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Transactions", "ScheduledTransactionId", c => c.Int());
            CreateIndex("accounts.Transactions", "ScheduledTransactionId");
            AddForeignKey("accounts.Transactions", "ScheduledTransactionId", "accounts.ScheduledTransactions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("accounts.Transactions", "ScheduledTransactionId", "accounts.ScheduledTransactions");
            DropIndex("accounts.Transactions", new[] { "ScheduledTransactionId" });
            DropColumn("accounts.Transactions", "ScheduledTransactionId");
        }
    }
}
