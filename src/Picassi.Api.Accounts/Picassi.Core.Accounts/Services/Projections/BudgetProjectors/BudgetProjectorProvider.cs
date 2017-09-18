using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Services.Projections.BudgetProjectors
{
    public interface IBudgetProjectorProvider
    {
        IBudgetProjector GetProjectorForFlow(Budget flow);
    }

    public class BudgetProjectorProvider : IBudgetProjectorProvider
    {
        private readonly IEnumerable<IBudgetProjector> _budgetProjectors;

        public BudgetProjectorProvider(IEnumerable<IBudgetProjector> budgetProjectors)
        {
            _budgetProjectors = budgetProjectors;
        }

        public IBudgetProjector GetProjectorForFlow(Budget budget)
        {
            return _budgetProjectors.Single(x => x.Type == budget.Period);
        }
    }
}
