using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Picassi.Data.Accounts.Models;
using Picassi.Utils.Data;

namespace Picassi.Data.Accounts.Database
{
    public interface IAccountsDataContext : IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Budget> Budgets { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<Goal> Goals { get; set; }
        DbSet<ModelledTransaction> ModelledTransactions { get; set; }
        DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        System.Data.Entity.Database Database { get; }
        DbRawSqlQuery<T> Query<T>(string sql, params object[] parms);
    }

    public class AccountsDataContext : DbContext, IAccountsDataContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<ModelledTransaction> ModelledTransactions { get; set; }
        public DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbRawSqlQuery<T> Query<T>(string sql, params object[] parms)
        {
            return Database.SqlQuery<T>(sql, parms);
        }

        public AccountsDataContext() : base("Accounts") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("accounts");
        }
    }
}
