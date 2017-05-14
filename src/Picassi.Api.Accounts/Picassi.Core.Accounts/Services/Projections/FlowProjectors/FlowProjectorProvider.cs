using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public interface IFlowProjectorProvider
    {
        IFlowProjector GetProjectorForFlow(ScheduledTransaction flow);
    }

    public class FlowProjectorProvider : IFlowProjectorProvider
    {
        private readonly IEnumerable<IFlowProjector> _flowProjectors;

        public FlowProjectorProvider(IEnumerable<IFlowProjector> flowProjectors)
        {
            _flowProjectors = flowProjectors;
        }

        public IFlowProjector GetProjectorForFlow(ScheduledTransaction flow)
        {
            return _flowProjectors.Single(x => x.Type == flow.Type);
        }
    }
}
