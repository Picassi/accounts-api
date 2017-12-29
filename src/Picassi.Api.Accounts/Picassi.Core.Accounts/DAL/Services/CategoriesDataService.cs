using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public override CategoryModel Get(int id)
        {
            var entity = DbProvider.GetDataContext().Categories
                .Include("Parent")
                .Include("Children")
                .SingleOrDefault(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException<Transaction>(id);

            return ModelMapper.Map(entity);
        }

        public IEnumerable<CategoryModel> Query(CategoriesQueryModel query)
        {
            var queryResults = DbProvider.GetDataContext().Categories
                .Include("Parent")
                .Include("Children")
                .AsQueryable();

            if (query?.Ids != null && query.Ids.Length > 0)
            {                
                queryResults = queryResults.Where(x => query.Ids.Contains(x.Id) || 
                    (query.IncludeSubCategories == true && x.ParentId != null && query.Ids.Contains((int)x.ParentId)));
            }

            if (query?.Name != null)
            {
                queryResults = queryResults.Where(x => x.Name.Contains(query.Name));
            }

            if (query?.CategoryType != null)
            {
                queryResults = queryResults.Where(x => x.CategoryType == query.CategoryType);
            }

            if (query?.ParentId != null)
            {
                queryResults = queryResults.Where(x => x.ParentId == query.ParentId);
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

        public override CategoryModel Update(int id, CategoryModel model)
        {
            var entity = DbProvider.GetDataContext().Categories.Find(id);
            if (entity == null) throw new EntityNotFoundException<Category>(id);

            ModelMapper.Patch(model, entity);
            if (model.ParentId != null) ValidateCategoryType((int)model.ParentId, model.CategoryType);
            DbProvider.GetDataContext().SaveChanges();

            UpdateChildCategoriesWithType(model.Id, model.CategoryType);

            return Get(id);
        }

        private void ValidateCategoryType(int categoryId, CategoryType type)
        {
            var category = DbProvider.GetDataContext().Categories.Find(categoryId);
            if (category.CategoryType != type) throw new ValidationException();
        }

        private void UpdateChildCategoriesWithType(int modelId, CategoryType type)
        {
            var childCategories = DbProvider.GetDataContext().Categories
                .Where(c => c.ParentId == modelId && c.CategoryType != type).ToList();

            foreach (var category in childCategories)
            {
                category.CategoryType = type;
                DbProvider.GetDataContext().SaveChanges();
                UpdateChildCategoriesWithType(category.Id, type);
            }
        }

        public override bool Delete(int id)
        {
            var entity = DbProvider.GetDataContext().Categories.Find(id);
            if (entity == null) throw new EntityNotFoundException<Category>(id);

            var transactions = DbProvider.GetDataContext().Transactions.Where(t => t.CategoryId == id);
            foreach (var t in transactions) t.CategoryId = null;

            var budgets = DbProvider.GetDataContext().Budgets.Where(t => t.CategoryId == id);
            foreach (var b in budgets) DbProvider.GetDataContext().Budgets.Remove(b);

            var rules = DbProvider.GetDataContext().AssignmentRules.Where(t => t.CategoryId == id);
            foreach (var r in rules) DbProvider.GetDataContext().AssignmentRules.Remove(r);

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
