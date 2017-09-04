using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Generator.Accounts.Generators
{
    public interface ITransactionGenerator
    {
        void AddTransactions(AccountModel account, CategoryModel category, int frequency, int period,
            string description, decimal amount);
    }

    public class TransactionGenerator : ITransactionGenerator
    {
        private readonly ITransactionsDataService _transactionsDataService;
        private readonly IAccountsDatabaseProvider _dbProvider;

        public TransactionGenerator(ITransactionsDataService transactionsDataService, IAccountsDatabaseProvider dbProvider)
        {
            _transactionsDataService = transactionsDataService;
            _dbProvider = dbProvider;
        }

        public void AddTransactions(AccountModel account, CategoryModel category, int frequency, int period, string description, decimal amount)
        {
            for (var i = 0; i < period; i += frequency)
            {
                _transactionsDataService.Create(new TransactionModel
                {
                    AccountId = account.Id,
                    AccountName = account.Name,
                    Amount = amount,
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    Date = DateTime.Today.AddDays(i - 90),
                    Description = description,
                    Ordinal = 1
                });
            }

            var ordinal = 1;
            foreach (var transaction in _dbProvider.GetDataContext().Transactions.OrderBy(x => x.Date)
                .ThenBy(x => x.Ordinal))
            {
                transaction.Ordinal = ordinal;
                ordinal++;
            }
            _dbProvider.GetDataContext().SaveChanges();
        }

    }
}
