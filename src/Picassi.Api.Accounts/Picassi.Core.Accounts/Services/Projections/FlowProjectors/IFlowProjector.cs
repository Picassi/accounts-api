using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Enums;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public interface IFlowProjector
    {
        FlowUserType Type { get; }
        IEnumerable<ModelledTransaction> GetProjectionsForFlow(ScheduledTransaction flow, DateTime start, DateTime end);
    }
}
