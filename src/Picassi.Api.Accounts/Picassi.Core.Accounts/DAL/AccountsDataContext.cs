using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Utils.Data;

namespace Picassi.Core.Accounts.DAL
{
    public class AccountsDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<ModelledTransaction> ModelledTransactions { get; set; }
        public DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbRawSqlQuery<T> Query<T>(string sql, params object[] parms)
        {
            return Database.SqlQuery<T>(sql, parms);
        }

        public AccountsDataContext()
        {
            
        }

        public AccountsDataContext(string connectionString) : base(connectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("accounts");
        }
    }
}
