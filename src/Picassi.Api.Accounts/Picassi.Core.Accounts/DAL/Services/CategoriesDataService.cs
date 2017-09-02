using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.Extensions;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ICategoriesDataService : IGenericDataService<CategoryModel>
    {
        IEnumerable<CategoryModel> Query(CategoriesQueryModel query);
    }

    public class CategoriesDataService : GenericDataService<CategoryModel, Category>, ICategoriesDataService
    {
        public CategoriesDataService(IModelMapper<CategoryModel, Category> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<CategoryModel> Query(CategoriesQueryModel query)
        {
            var queryResults = DbProvider.GetDataContext().Categories.AsQueryable();

            if (query?.Ids != null && query.Ids.Length > 0)
            {
                queryResults = queryResults.Where(x => query.Ids.Contains(x.Id));
            }

            if (query?.Name != null)
            {
                queryResults = queryResults.Where(x => x.Name.Contains(query.Name));
            }


            queryResults = query == null ? queryResults : OrderResults(queryResults, query.SortBy, query.SortAscending);
            return ModelMapper.MapList(queryResults);
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
