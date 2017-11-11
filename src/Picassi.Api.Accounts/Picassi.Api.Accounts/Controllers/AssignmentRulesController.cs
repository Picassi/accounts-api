using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.AssignmentRules;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]    
    public class AssignmentRulesController : ApiController
	{
        private readonly IAssignmentRulesDataService _dataService;

	    public AssignmentRulesController(IAssignmentRulesDataService dataService)
	    {
	        _dataService = dataService;
	    }

	    [HttpGet]
        [Route("rules")]
        public IEnumerable<AssignmentRuleModel> GetAssignmentRules([FromUri]AssignmentRuleQueryModel query)
        {
            return _dataService.Query(query?.PageNumber ?? 1, query?.PageSize ?? 20, 
                search: query?.Text, types: query?.Types, accountIds: query?.Accounts, categoryIds: query?.Categories).ToList();
        }

        [HttpPost]
        [Route("rules")]
        public AssignmentRuleModel CreateAssignmentRule([FromBody]AssignmentRuleModel assignmentRuleModel)
        {
            return _dataService.Create(assignmentRuleModel);
        }

        [HttpGet]
        [Route("rules/{id}")]
        public AssignmentRuleModel GetAssignmentRule(int id)
        {
            return _dataService.Get(id);            
        }

	    [HttpPut]
        [Route("rules/{id}")]
        public AssignmentRuleModel UpdateAssignmentRule(int id, [FromBody]AssignmentRuleModel assignmentRuleModel)
        {
            return _dataService.Update(id, assignmentRuleModel);
        }

        [HttpDelete]
        [Route("rules/{id}")]
        public bool DeleteAssignmentRule(int id)
        {
            return _dataService.Delete(id);
        }
    }
}