using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Services.Reports;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class CategoriesController : ApiController
    {
        private readonly ICategoriesDataService _dataService;
        private readonly ICategorySummariser _categorySummariser;
        private readonly IBudgetsDataService _budgetsDataService;

        public CategoriesController(ICategoriesDataService dataService, ICategorySummariser categorySummariser, IBudgetsDataService budgetsDataService)
        {
            _dataService = dataService;
            _categorySummariser = categorySummariser;
            _budgetsDataService = budgetsDataService;
        }

        [HttpGet]
        [Route("categories")]
        public IEnumerable<CategoryModel> GetCategories([FromUri]CategoriesQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpGet]
        [Route("categories/summary")]
        public IEnumerable<CategorySummaryViewModel> GetCategoriesSumary([FromUri]CategoriesQueryModel query)
        {
            return _categorySummariser.GetCategorySummaries(query?.DateFrom, query?.DateTo);
        }

        [HttpPost]
        [Route("categories")]
        public CategoryModel CreateCategory([FromBody]CategoryModel model)
        {
            if (string.IsNullOrEmpty(model?.Name))
            {
                throw new InvalidOperationException("Cannot create category with empty name");
            }
            var existingCategory = _dataService.Query(new CategoriesQueryModel { Name = model.Name }).FirstOrDefault();
            return existingCategory ?? _dataService.Create(model);
        }

        [HttpPost]
        [Route("category-and-budget")]
        public CategoryModel CreateCategoryWithBudget([FromBody]CategoryWithBudget model)
        {
            if (string.IsNullOrEmpty(model?.Category?.Name))
            {
                throw new InvalidOperationException("Cannot create category with empty name");
            }

            var existingCategory = _dataService.Query(new CategoriesQueryModel { Name = model.Category.Name }).FirstOrDefault();
            if (existingCategory != null)
            {
                throw new InvalidOperationException($"Category {existingCategory.Name} already exists");
            }

            var categoryModel = _dataService.Create(model.Category);

            if (model.Budget != null)
            {
                model.Budget.CategoryId = categoryModel.Id;
                model.Budget.AggregationPeriod = 1;
                _budgetsDataService.Create(model.Budget);
            }

            return categoryModel;
        }

        [HttpGet]
        [Route("categories/{id}")]
        public CategoryModel GetCategory(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("categories/{id}")]
        public CategoryModel UpdateCategory(int id, [FromBody]CategoryModel categoryModel)
        {
            return _dataService.Update(id, categoryModel);
        }

        [HttpDelete]
        [Route("categories/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}