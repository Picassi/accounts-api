using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;

namespace Picassi.Generator.Accounts.Generators
{
    public class WagesIncomeGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Transaction);

        private readonly IAccountDataService _accountDataService;
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ITransactionGenerator _transactionGenerator;

        public WagesIncomeGenerator(IAccountDataService accountDataService, ICategoriesDataService categoriesDataService, ITransactionGenerator transactionGenerator)
        {
            _accountDataService = accountDataService;
            _categoriesDataService = categoriesDataService;
            _transactionGenerator = transactionGenerator;
        }


        public void Generate(DataGenerationContext context)
        {
            var account = _accountDataService.Query(new AccountQueryModel { Name = "Main" }).Single();
            var category = _categoriesDataService.Query(new CategoriesQueryModel { Name = "Wages" }).Single();

            _transactionGenerator.AddTransactions(account, category, 30, 90, "Salary", (decimal)2524);
        }
    }
}
