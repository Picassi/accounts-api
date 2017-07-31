using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public TransactionGenerator(ITransactionsDataService transactionsDataService)
        {
            _transactionsDataService = transactionsDataService;
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
                    Ordinal = 1,
                });
            }
        }

    }
}
