using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.Goals;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
{
    public interface IGoalsDataService : IGenericDataService<GoalModel>
    {
        IEnumerable<GoalModel> Query(GoalsQueryModel query);
    }

    public class GoalsDataService : GenericDataService<GoalModel, Goal>, IGoalsDataService
    {
        public GoalsDataService(IModelMapper<GoalModel, Goal> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<GoalModel> Query(GoalsQueryModel query)
        {
            var queryResults = DbContext.Goals.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
