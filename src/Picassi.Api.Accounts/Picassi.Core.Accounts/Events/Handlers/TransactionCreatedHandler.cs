using Picassi.Data.Accounts.Events.Messages;
using Picassi.Data.Accounts.Models;

namespace Picassi.Data.Accounts.Events.Handlers
{
    public class TransactionCreatedHandler : IEventHandler<EntityCreated<Transaction>>
    {
        public void Handle(EntityCreated<Transaction> @event)
        {
            
        }
    }
}
