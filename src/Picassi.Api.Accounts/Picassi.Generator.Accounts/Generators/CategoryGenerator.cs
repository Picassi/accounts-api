using System;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Categories;

namespace Picassi.Generator.Accounts.Generators
{
    public class CategoryGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Category);

        private readonly string[] _categories = {
            "Food", "Bills", "Leisure", "Transport", "Wages"
        };

        private readonly ICategoriesDataService _dataService;

        public CategoryGenerator(ICategoriesDataService dataService)
        {
            _dataService = dataService;
        }

        public void Generate(DataGenerationContext context)
        {
            foreach (var category in _categories)
            {
                _dataService.Create(new CategoryModel
                {
                    Name = category
                });
            }
        }
    }
}
