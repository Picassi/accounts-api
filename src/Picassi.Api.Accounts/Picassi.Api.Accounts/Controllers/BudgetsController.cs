﻿using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Budgets;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class BudgetsController : ApiController
    {
        private readonly IBudgetsDataService _dataService;

        public BudgetsController(IBudgetsDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("budgets")]
        public IEnumerable<BudgetModel> GetBudgets([FromUri]BudgetsQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("budgets")]
        public BudgetModel CreateBudget([FromBody]BudgetModel model)
        {
            return _dataService.Create(model);
        }

        [HttpGet]
        [Route("budgets/{id}")]
        public BudgetModel GetBudget(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("budgets/{id}")]
        public BudgetModel UpdateBudget(int id, [FromBody]BudgetModel budgetModel)
        {
            return _dataService.Update(id, budgetModel);
        }

        [HttpDelete]
        [Route("budgets/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}