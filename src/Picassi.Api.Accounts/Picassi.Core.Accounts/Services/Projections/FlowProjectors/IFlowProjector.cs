using System;
using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public interface IFlowProjector
    {
        PeriodType Type { get; }
        IEnumerable<ModelledTransaction> GetProjectionsForFlow(ScheduledTransaction flow, DateTime start, DateTime end);
    }
}
