using System;
using Picassi.Data.Accounts.Events;

namespace Picassi.Core.Accounts.Events.Messages
{
    public class TransactionMoved : IEvent
    {
        public int AccountId { get; }
        public DateTime Date { get; }

        public TransactionMoved(int accountId, DateTime date)
        {
            AccountId = accountId;
            Date = date;
        }
    }
}
