namespace Picassi.Data.Accounts.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddBalanceToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("accounts.Transactions", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("accounts.Transactions", "Balance");
        }
    }
}
