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
        DbSet<Tag> Tags { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<ReportGroupReport> ReportGroupReports { get; set; }
        DbSet<ReportGroup> ReportGroups { get; set; }
        DbSet<ReportGroupCategory> ReportGroupCategories { get; set; }
    }

    public class AccountsDataContext : DbContext, IAccountsDataContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportGroupReport> ReportGroupReports { get; set; }
        public DbSet<ReportGroup> ReportGroups { get; set; }
        public DbSet<ReportGroupCategory> ReportGroupCategories { get; set; }

        public AccountsDataContext() : base("Accounts") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("accounts");
        }
    }
}
