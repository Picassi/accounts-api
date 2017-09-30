using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.AssignmentRules;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IAssignmentRulesDataService : IGenericDataService<AssignmentRuleModel>
    {
        IEnumerable<AssignmentRuleModel> Query(int pageNumber = 1, int pageSize = 20);
    }

    public class AssignmentRulesDataService : GenericDataService<AssignmentRuleModel, AssignmentRule>, IAssignmentRulesDataService
    {
        public AssignmentRulesDataService(IModelMapper<AssignmentRuleModel, AssignmentRule> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<AssignmentRuleModel> Query(int pageNumber = 1, int pageSize = 20)
        {
            var skip = (pageNumber - 1) * pageSize;
            var queryResults = DbProvider.GetDataContext().AssignmentRules
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.CreatedDate)
                .Skip(skip).Take(pageNumber).AsQueryable();
            return queryResults.Select(ModelMapper.Map);
        }
    }
}
