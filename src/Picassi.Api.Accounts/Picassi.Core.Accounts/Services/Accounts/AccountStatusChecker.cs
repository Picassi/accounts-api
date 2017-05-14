using System;
using System.Linq;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Core.Accounts.Services.Accounts
{
    public interface IAccountStatusChecker
    {
        DateTime? LastUpdated(int accountId);
        DateTime? LastUpdated();
        DateTime? MostOutOfDate();
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
            return _dataContext.Accounts.Find(accountId).LastUpdated;
        }

        public DateTime? LastUpdated()
        {
            return _dataContext.Accounts.Any() ? _dataContext.Accounts.Max(account => account.LastUpdated) : (DateTime?)null;
        }

        public DateTime? MostOutOfDate()
        {
            return _dataContext.Accounts.Any() ? _dataContext.Accounts.Min(account => account.LastUpdated) : (DateTime?)null;
        }
    }
}
