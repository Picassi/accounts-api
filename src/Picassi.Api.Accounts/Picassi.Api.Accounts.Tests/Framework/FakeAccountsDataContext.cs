using System;
using System.Data.Entity;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class FakeAccountsDataContext : IAccountsDataContext
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
        public Database Database { get; }

        public FakeAccountsDataContext()
        {
            Accounts = new FakeDbSet<Account>();
            AssignmentRules = new FakeDbSet<AssignmentRule>();
            Budgets = new FakeDbSet<Budget>();
            Categories = new FakeDbSet<Category>();
            Events = new FakeDbSet<Event>();
            Goals = new FakeDbSet<Goal>();
            ModelledTransactions = new FakeDbSet<ModelledTransaction>();
            ScheduledTransactions = new FakeDbSet<ScheduledTransaction>();
            Tags = new FakeDbSet<Tag>();
            Transactions = new FakeDbSet<Transaction>();
        }

        public IDbSet<TModel> GetDbSet<TModel>() where TModel : class
        {
            var property = typeof(FakeAccountsDataContext).GetProperties().SingleOrDefault(x =>
                x.PropertyType.GenericTypeArguments.Contains(typeof(TModel)));

            return property == null ? null : (IDbSet<TModel>)property.GetValue(this);
        }

        public int SaveChanges()
        {
            return 0;
        }
    }

    public class FakeDatabaseProvider : IAccountsDatabaseProvider
    {
        public IAccountsDataContext Database { get; }

        public FakeDatabaseProvider()
        {
            Database = new FakeAccountsDataContext();
        }


        public IAccountsDataContext GetDataContext()
        {
            return Database;
        }
    }
}
