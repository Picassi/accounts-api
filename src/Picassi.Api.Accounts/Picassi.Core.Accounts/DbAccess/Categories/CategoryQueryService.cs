using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Common.Data.Extensions;
using Picassi.Core.Accounts.ViewModels.Categories;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Categories
{
    public interface ICategoryQueryService
    {
        IEnumerable<CategoryViewModel> Query(CategoriesQueryModel accounts);
    }

    public class CategoryQueryService : ICategoryQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public CategoryQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<CategoryViewModel> Query(CategoriesQueryModel query)
        {			
            var queryResults = _dbContext.Categories.AsQueryable();

            if (query?.Name != null) queryResults = queryResults.Where(x => x.Name.Contains(query.Name));
            queryResults = query == null ? queryResults : OrderResults(queryResults, query.SortBy, query.SortAscending);
            return Mapper.Map<IEnumerable<CategoryViewModel>>(queryResults);
        }

        private static IQueryable<Category> OrderResults(IQueryable<Category> categories, string field, bool ascending)
        {
            if (field == null)
            {
                field = "Id";
                @ascending = true;
            }

            return field == "Id" ? categories.OrderBy(field, @ascending) : categories.OrderBy(field, @ascending).ThenBy("Id", @ascending);
        }
    }
}
