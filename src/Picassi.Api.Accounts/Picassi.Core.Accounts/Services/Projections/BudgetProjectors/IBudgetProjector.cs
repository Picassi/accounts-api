using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Projections.BudgetProjectors
{
    public interface IBudgetProjector
    {
        PeriodType Type { get; }
        IEnumerable<ModelledTransaction> GetProjectionsForBudget(Budget budget, DateTime start, DateTime end, IList<ModelledTransaction> modelledTransactions);
    }
}
