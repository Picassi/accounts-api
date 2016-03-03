using System.Data.Entity;
using Picassi.Common.Data;
using Picassi.Data.Accounts.Models;

namespace Picassi.Data.Accounts.Database
{
    public interface IAccountsDataContext : IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Snapshot> Snapshots { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<Category> Categories { get; set; }
    }

    public class AccountsDataContext : DbContext, IAccountsDataContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AccountsDataContext() : base("Accounts") { }
    }
}
