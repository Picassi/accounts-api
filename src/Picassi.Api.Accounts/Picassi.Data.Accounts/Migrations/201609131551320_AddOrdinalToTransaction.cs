namespace Picassi.Data.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrdinalToTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Transactions", "Ordinal", c => c.Int(nullable: false));

            Sql("Update accounts.Transactions SET Ordinal = (SELECT Count(*) FROM accounts.Transactions trans2 WHERE trans2.id < accounts.Transactions.ID) + 1");
        }

        public override void Down()
        {
            DropColumn("accounts.Transactions", "Ordinal");
        }
    }
}
