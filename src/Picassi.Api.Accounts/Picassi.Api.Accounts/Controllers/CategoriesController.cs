using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Common.Data.Enums;
using Picassi.Core.Accounts.DbAccess.Categories;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.Time.Periods;
using Picassi.Core.Accounts.ViewModels.Categories;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryCrudService _crudService;
        private readonly ICategoryQueryService _queryService;
        private readonly ICategorySummaryService _categorySummaryService;

        public CategoriesController(ICategoryCrudService crudService, ICategoryQueryService queryService, ICategorySummaryService categorySummaryService)
        {
            _crudService = crudService;
            _queryService = queryService;
            _categorySummaryService = categorySummaryService;
        }

        [HttpGet]
        [Route("categories")]
        public IEnumerable<CategoryViewModel> GetCategories([FromUri]CategoriesQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpGet]
        [Route("categories/summary/{frequency}")]
        public IEnumerable<CategorySummaryViewModel> GetCategoriesSumary(string frequency, [FromUri]CategoriesQueryModel query)
        {
            var frequencyValue = (PeriodType)Enum.Parse(typeof (PeriodType), frequency, true);
            return _categorySummaryService.GetCategorySummaries(query, frequencyValue);
        }

        [HttpPost]
        [Route("categories")]
        public CategoryViewModel CreateCategory([FromBody]CategoryViewModel CategoryViewModel)
        {
            return _crudService.CreateCategory(CategoryViewModel);
        }

        [HttpGet]
        [Route("categories/{id}")]
        public CategoryViewModel GetCategory(int id)
        {
            return _crudService.GetCategory(id);
        }

        [HttpPut]
        [Route("categories/{id}")]
        public CategoryViewModel UpdateCategory(int id, [FromBody]CategoryViewModel CategoryViewModel)
        {
            return _crudService.UpdateCategory(id, CategoryViewModel);
        }

        [HttpDelete]
        [Route("categories/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteCategory(id);
        }
    }
}