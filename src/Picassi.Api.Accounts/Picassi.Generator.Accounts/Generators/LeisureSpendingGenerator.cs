using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Generator.Accounts.Generators
{
    public class LeisureSpendingGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Transaction);

        private readonly IAccountDataService _accountDataService;
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ITransactionGenerator _transactionGenerator;

        public LeisureSpendingGenerator(IAccountDataService accountDataService, ICategoriesDataService categoriesDataService, ITransactionGenerator transactionGenerator)
        {
            _accountDataService = accountDataService;
            _categoriesDataService = categoriesDataService;
            _transactionGenerator = transactionGenerator;
        }


        public void Generate(DataGenerationContext context)
        {
            var account = _accountDataService.Query(new AccountQueryModel {Name = "Main"}).Single();
            var category = _categoriesDataService.Query(new CategoriesQueryModel {Name = "Leisure" }).Single();

            _transactionGenerator.AddTransactions(account, category, 10, 90, "Cinema", (decimal)28.60);
            _transactionGenerator.AddTransactions(account, category, 14, 90, "After Work Beers", (decimal)32.44);
            _transactionGenerator.AddTransactions(account, category, 28, 90, "Theatre", (decimal)73.50);
        }
    }
}
