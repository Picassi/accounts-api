using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Goals;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IGoalsDataService : IGenericDataService<GoalModel>
    {
        IEnumerable<GoalModel> Query(GoalsQueryModel query);
    }

    public class GoalsDataService : GenericDataService<GoalModel, Goal>, IGoalsDataService
    {
        public GoalsDataService(IModelMapper<GoalModel, Goal> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<GoalModel> Query(GoalsQueryModel query)
        {
            var queryResults = DbProvider.GetDataContext().Goals.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
