using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Exceptions;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.Extensions;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ICategoriesDataService : IGenericDataService<CategoryModel>
    {
        IEnumerable<CategoryModel> Query(CategoriesQueryModel query);
        IEnumerable<CategoryGroupModel> GetCategoryGroups(CategoriesQueryModel query);
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

        public IEnumerable<CategoryGroupModel> GetCategoryGroups(CategoriesQueryModel query)
        {
            return Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().Select(c => new CategoryGroupModel
            {
                Name = c.ToString()
            });
        }

        public override bool Delete(int id)
        {
            var entity = DbProvider.GetDataContext().Categories.Find(id);
            if (entity == null) throw new EntityNotFoundException<Category>(id);

            var transactions = DbProvider.GetDataContext().Transactions.Where(t => t.CategoryId == id);
            foreach (var t in transactions) t.CategoryId = null;

            var budgets = DbProvider.GetDataContext().Budgets.Where(t => t.CategoryId == id);
            foreach (var b in budgets) DbProvider.GetDataContext().Budgets.Remove(b);

            DbProvider.GetDataContext().Categories.Remove(entity);
            DbProvider.GetDataContext().SaveChanges();
            return true;
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
