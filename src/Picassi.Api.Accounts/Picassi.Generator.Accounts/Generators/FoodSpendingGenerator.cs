using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Generator.Accounts.Generators
{
    public class FoodSpendingGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Transaction);

        private readonly IAccountDataService _accountDataService;
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ITransactionGenerator _transactionGenerator;

        public FoodSpendingGenerator(IAccountDataService accountDataService, ICategoriesDataService categoriesDataService, ITransactionGenerator transactionGenerator)
        {
            _accountDataService = accountDataService;
            _categoriesDataService = categoriesDataService;
            _transactionGenerator = transactionGenerator;
        }


        public void Generate(DataGenerationContext context)
        {
            var account = _accountDataService.Query(new AccountQueryModel {Name = "Main"}).Single();
            var category = _categoriesDataService.Query(new CategoriesQueryModel {Name = "Food"}).Single();

            _transactionGenerator.AddTransactions(account, category, 3, 90, "Local Food Shop", -(decimal)13.22);
            _transactionGenerator.AddTransactions(account, category, 7, 90, "Weekly Shop", -(decimal)50.23);
            _transactionGenerator.AddTransactions(account, category, 6, 90, "Takeaway", -(decimal)24.20);            
        }
    }
}
