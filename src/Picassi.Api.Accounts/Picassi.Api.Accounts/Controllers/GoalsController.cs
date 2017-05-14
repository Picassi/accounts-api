using System.Collections.Generic;
using System.Web.Http;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Goals;

namespace Picassi.Api.Accounts.Controllers
{
    public class GoalsController : ApiController
    {
        private readonly IGoalsDataService _dataService;

        public GoalsController(IGoalsDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("goals")]
        public IEnumerable<GoalModel> GetGoals([FromUri]GoalsQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("goals")]
        public GoalModel CreateGoal([FromBody]GoalModel model)
        {
            return _dataService.Create(model);
        }

        [HttpGet]
        [Route("goals/{id}")]
        public GoalModel GetGoal(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("goals/{id}")]
        public GoalModel UpdateGoal(int id, [FromBody]GoalModel model)
        {
            return _dataService.Update(id, model);
        }

        [HttpDelete]
        [Route("goals/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}