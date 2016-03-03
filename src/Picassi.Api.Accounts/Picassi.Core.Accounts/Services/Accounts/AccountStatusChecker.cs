using System;
using System.Linq;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.Services.Accounts
{
    public interface IAccountStatusChecker
    {
        DateTime? LastUpdated(int accountId);
    }

    public class AccountStatusChecker : IAccountStatusChecker
    {
        private readonly IAccountsDataContext _dataContext;

        public AccountStatusChecker(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public DateTime? LastUpdated(int accountId)
        {
            return _dataContext.Transactions.Any() ? _dataContext.Transactions.Max(x => x.Date) : (DateTime?) null;
        }
    }
}
