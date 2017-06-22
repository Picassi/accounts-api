using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Categories;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ICategorySummaryService
    {
        CategorySummaryResultsViewModel GetCategorySummaries(CategoriesQueryModel query);
    }

    public class CategorySummaryService : ICategorySummaryService
    {
        private readonly IAccountsDataContext _dataContext;

        public CategorySummaryService(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public CategorySummaryResultsViewModel GetCategorySummaries(CategoriesQueryModel query)
        {
            var filteredTransactions = GroupByFilteredAccounts(query);

            if (!query.AllAccounts)
            {
                filteredTransactions = filteredTransactions.Where(transaction => query.AccountIds.Contains(transaction.AccountId));
            }

            var groupedTransactions = filteredTransactions
                .GroupBy(transaction => transaction.Category)
                .ToList();

            var summaries = groupedTransactions
                .Select(grp => new CategorySummaryViewModel(grp.Key, grp.Select(results => results).ToList()));            

            return new CategorySummaryResultsViewModel(summaries.ToList());
        }

        private IQueryable<Transaction> GroupByFilteredAccounts(CategoriesQueryModel query)
        {
            var transactions = _dataContext.Transactions.Include("Category").AsQueryable();
            return query.AllAccounts
                ? transactions : transactions.Where(r => query.AccountIds != null && query.AccountIds.Contains(r.AccountId));
        }
    }
}
