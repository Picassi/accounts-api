using System;
using System.Collections.Generic;
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
    public class TagsController : ApiController
    {
        private readonly ICategoriesDataService _dataService;

        public TagsController(ICategoriesDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("tags")]
        public IEnumerable<CategoryModel> GetCategories([FromUri]CategoriesQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("tags")]
        public CategoryModel CreateCategory([FromBody]CategoryModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new InvalidOperationException("Cannot create tag with empty name");
            }
            var existingCategory = _dataService.Query(new CategoriesQueryModel { Name = model.Name }).FirstOrDefault();
            return existingCategory ?? _dataService.Create(model);
        }

        [HttpGet]
        [Route("tags/{id}")]
        public CategoryModel GetCategory(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("tags/{id}")]
        public CategoryModel UpdateCategory(int id, [FromBody]CategoryModel tagModel)
        {
            return _dataService.Update(id, tagModel);
        }

        [HttpDelete]
        [Route("tags/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}