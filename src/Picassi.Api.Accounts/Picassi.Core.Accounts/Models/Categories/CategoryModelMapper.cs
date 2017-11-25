using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategoryModelMapper : IModelMapper<CategoryModel, Category>
    {
        public Category CreateEntity(CategoryModel model)
        {
            return new Category
            {
                Id = model.Id,
                Name = model.Name,
                ParentId = model.ParentId,
                CategoryType = model.CategoryType
            };
        }

        public CategoryModel Map(Category model)
        {
            return new CategoryModel
            {
                Id = model.Id,
                Name = model.Name,
                ParentId = model.ParentId,
                ParentName = model.Parent?.Name,
                CategoryType = model.CategoryType,
                HasChildren = model.Children != null && model.Children.Any()
            };
        }

        public void Patch(CategoryModel model, Category entity)
        {
            entity.Name = model.Name;
            entity.ParentId = model.ParentId;
            entity.CategoryType = model.CategoryType;
        }

        public IEnumerable<CategoryModel> MapList(IEnumerable<Category> results)
        {
            return results.Select(Map);
        }
    }
}