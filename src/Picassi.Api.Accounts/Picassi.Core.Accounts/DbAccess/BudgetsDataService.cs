using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.Budgets;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
{
    public interface IBudgetsDataService : IGenericDataService<BudgetModel>
    {
        IEnumerable<BudgetModel> Query(BudgetsQueryModel query);
    }

    public class BudgetsDataService : GenericDataService<BudgetModel, Budget>, IBudgetsDataService
    {
        public BudgetsDataService(IModelMapper<BudgetModel, Budget> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<BudgetModel> Query(BudgetsQueryModel query)
        {
            var queryResults = DbContext.Budgets.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
