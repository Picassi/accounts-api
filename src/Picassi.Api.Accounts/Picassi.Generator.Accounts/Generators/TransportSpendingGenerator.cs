using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Generator.Accounts.Generators
{
    public class TransportSpendingGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Transaction);

        private readonly IAccountDataService _accountDataService;
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ITransactionGenerator _transactionGenerator;

        public TransportSpendingGenerator(IAccountDataService accountDataService, ICategoriesDataService categoriesDataService, ITransactionGenerator transactionGenerator)
        {
            _accountDataService = accountDataService;
            _categoriesDataService = categoriesDataService;
            _transactionGenerator = transactionGenerator;
        }


        public void Generate(DataGenerationContext context)
        {
            var account = _accountDataService.Query(new AccountQueryModel {Name = "Main"}).Single();
            var category = _categoriesDataService.Query(new CategoriesQueryModel {Name = "Transport"}).Single();

            _transactionGenerator.AddTransactions(account, category, 3, 90, "Oyster Card Top Up", 20);
            _transactionGenerator.AddTransactions(account, category, 21, 90, "Train Fare", (decimal)48.65);
        }
    }
}
