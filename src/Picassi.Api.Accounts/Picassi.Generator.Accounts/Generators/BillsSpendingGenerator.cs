using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;

namespace Picassi.Generator.Accounts.Generators
{
    public class BillsSpendingGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Transaction);

        private readonly IAccountDataService _accountDataService;
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ITransactionGenerator _transactionGenerator;

        public BillsSpendingGenerator(IAccountDataService accountDataService, ICategoriesDataService categoriesDataService, ITransactionGenerator transactionGenerator)
        {
            _accountDataService = accountDataService;
            _categoriesDataService = categoriesDataService;
            _transactionGenerator = transactionGenerator;
        }


        public void Generate(DataGenerationContext context)
        {
            var account = _accountDataService.Query(new AccountQueryModel {Name = "Main"}).Single();
            var category = _categoriesDataService.Query(new CategoriesQueryModel {Name = "Bills"}).Single();

            _transactionGenerator.AddTransactions(account, category, 30, 90, "Rent", -(decimal)1300);
            _transactionGenerator.AddTransactions(account, category, 30, 90, "Gas & Electric", -(decimal)62.34);
            _transactionGenerator.AddTransactions(account, category, 30, 90, "Water", -(decimal)24.23);
            _transactionGenerator.AddTransactions(account, category, 30, 90, "Council Tax", -(decimal)116);
            _transactionGenerator.AddTransactions(account, category, 30, 90, "Internet", -(decimal)32.24);
        }
    }
}
