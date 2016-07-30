using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Categories;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Categories
{
    public interface ICategoryCrudService
    {
        CategoryViewModel CreateCategory(CategoryViewModel category);
        CategoryViewModel GetCategory(int id);
        CategoryViewModel UpdateCategory(int id, CategoryViewModel category);
        bool DeleteCategory(int id);
    }

    public class CategoryCrudService : ICategoryCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public CategoryCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public CategoryViewModel CreateCategory(CategoryViewModel category)
        {
            var dataModel = Mapper.Map<Category>(category);
            _dbContext.Categories.Add(dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<CategoryViewModel>(dataModel);
        }

        public CategoryViewModel GetCategory(int id)
        {
            var dataModel = _dbContext.Categories.Find(id);
            return Mapper.Map<CategoryViewModel>(dataModel);
        }

        public CategoryViewModel UpdateCategory(int id, CategoryViewModel category)
        {
            var dataModel = _dbContext.Categories.Find(id);
            Mapper.Map(category, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<CategoryViewModel>(dataModel);
        }

        public bool DeleteCategory(int id)
        {
            var dataModel = _dbContext.Categories.Find(id);
            foreach (var transaction in _dbContext.Transactions.Where(transaction => transaction.CategoryId == id))
                transaction.CategoryId = null;
            _dbContext.SaveChanges();

            _dbContext.Categories.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
