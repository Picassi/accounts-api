using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Categories;
using Picassi.Data.Accounts.Database;

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

        public IEnumerable<CategoryViewModel> Query(CategoriesQueryModel accounts)
        {			
            var queryResults = _dbContext.Categories.AsQueryable();

            if (accounts?.Name != null)
            {
                queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));
            }

            return Mapper.Map<IEnumerable<CategoryViewModel>>(queryResults);
        }
    }
}
