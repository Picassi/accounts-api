using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Api.Accounts.Contract
{
    public class SetTransactionRecurrenceRequest
    {
        public IList<int> TransactionIds { get; set; }
        public PeriodType? Recurrence { get; set; }
    }
}