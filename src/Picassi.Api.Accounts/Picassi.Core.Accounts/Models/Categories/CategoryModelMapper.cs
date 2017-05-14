using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategoryModelMapper : IModelMapper<CategoryModel, Category>
    {
        public Category CreateEntity(CategoryModel model)
        {
            return Mapper.Map<Category>(model);
        }

        public CategoryModel Map(Category model)
        {
            return Mapper.Map<CategoryModel>(model);
        }

        public void Patch(CategoryModel model, Category entity)
        {
            Mapper.Map(model, entity);
        }
    }
}