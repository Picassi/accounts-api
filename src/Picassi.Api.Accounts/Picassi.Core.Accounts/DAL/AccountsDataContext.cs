using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.DAL
{
    public interface IAccountsDataContext
    {
        IDbSet<Account> Accounts { get; set; }
        IDbSet<AssignmentRule> AssignmentRules { get; set; }
        IDbSet<Budget> Budgets { get; set; }
        IDbSet<Category> Categories { get; set; }
        IDbSet<Event> Events { get; set; }
        IDbSet<Goal> Goals { get; set; }
        IDbSet<ModelledTransaction> ModelledTransactions { get; set; }
        IDbSet<Note> Notes { get; set; }
        IDbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        IDbSet<Tag> Tags { get; set; }
        IDbSet<Task> Tasks { get; set; }
        IDbSet<Transaction> Transactions { get; set; }
        Database Database { get; }
        IDbSet<TModel> GetDbSet<TModel>() where TModel : class;
        int SaveChanges();
    }

    public class AccountsDataContext : DbContext, IAccountsDataContext
    {
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<AssignmentRule> AssignmentRules { get; set; }
        public IDbSet<Budget> Budgets { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<Goal> Goals { get; set; }
        public IDbSet<ModelledTransaction> ModelledTransactions { get; set; }
        public IDbSet<Note> Notes { get; set; }
        public IDbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        public AccountsDataContext() : base("Accounts")
        {
            
        }

        public AccountsDataContext(string connectionString) : base(connectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("accounts");
        }

        public IDbSet<TModel> GetDbSet<TModel>() where TModel : class
        {
            return Set<TModel>();
        }
    }
}
