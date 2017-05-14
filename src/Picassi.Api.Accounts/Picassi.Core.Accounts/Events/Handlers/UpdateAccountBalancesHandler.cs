using Picassi.Core.Accounts.Events.Messages;
using Picassi.Core.Accounts.Services.Reports;

namespace Picassi.Core.Accounts.Events.Handlers
{
    public class UpdateAccountBalancesHandler : IEventHandler<TransactionMoved>
    {
        private readonly IAccountBalanceService _accountBalanceService;

        public UpdateAccountBalancesHandler(IAccountBalanceService accountBalanceService)
        {
            _accountBalanceService = accountBalanceService;
        }

        public void Handle(TransactionMoved @event)
        {
            _accountBalanceService.SetTransactionBalances(@event.AccountId, @event.Date);
        }
    }
}
