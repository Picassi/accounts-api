using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Exceptions;
using Picassi.Core.Accounts.Models.AssignmentRules;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IAssignmentRulesDataService : IGenericDataService<AssignmentRuleModel>
    {
        IEnumerable<AssignmentRuleModel> Query(int pageNumber = 1, int pageSize = 20, string search = null, 
            AssignmentRuleType[] types = null, int[] accountIds = null, int[] categoryIds = null);
        IEnumerable<AssignmentRuleModel> GetRules(bool? enabled = null);
    }

    public class AssignmentRulesDataService : GenericDataService<AssignmentRuleModel, AssignmentRule>, IAssignmentRulesDataService
    {
        public AssignmentRulesDataService(IModelMapper<AssignmentRuleModel, AssignmentRule> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public override AssignmentRuleModel Get(int id)
        {
            var entity = DbProvider.GetDataContext().AssignmentRules
                .Include("Category")
                .SingleOrDefault(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException<AssignmentRule>(id);
            return ModelMapper.Map(entity);

        }

        public IEnumerable<AssignmentRuleModel> Query(int pageNumber = 1, int pageSize = 20, string search = null, 
            AssignmentRuleType[] types = null, int[] accountIds = null, int[] categoryIds = null)
        {
            var skip = (pageNumber - 1) * pageSize;
            var queryResults = DbProvider.GetDataContext().AssignmentRules
                .Include("Category")
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.CreatedDate)
                .Skip(skip).Take(pageSize).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                queryResults = queryResults.Where(x => x.DescriptionRegex.Contains(search));
            }

            if (types != null && types.Length > 0)
            {
                queryResults = queryResults.Where(x => types.Contains(x.Type));
            }

            if (categoryIds != null && categoryIds.Length > 0)
            {
                queryResults = queryResults.Where(x => x.CategoryId != null && categoryIds.Contains((int)x.CategoryId));
            }

            return queryResults.OrderBy(q => q.DescriptionRegex).ToList().Select(ModelMapper.Map);
        }

        public IEnumerable<AssignmentRuleModel> GetRules(bool? enabled = null)
        {
            var queryResults = DbProvider.GetDataContext().AssignmentRules.Include("Category");

            if (enabled != null) queryResults = queryResults.Where(x => x.Enabled == enabled);

            return queryResults.ToList().Select(ModelMapper.Map);
        }
    }
}
