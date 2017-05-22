using System;
using System.Linq;
using System.Web;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.Models.Meta;
using Picassi.Core.Accounts.Services.Reports;
using Picassi.Utils.Data.Identity;

namespace Picassi.Core.Accounts.Services.Meta
{
    public interface IMetaDataService
    {
        MetaDataViewModel GetMetaData();
    }

    public class MetaDataService : IMetaDataService
    {
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly IAccountsDataContext _dataContext;
        private readonly IUserIdentityProvider _userIdentityProvider;

        public MetaDataService(IAccountBalanceService accountBalanceService, IAccountsDataContext dataContext, IUserIdentityProvider userIdentityProvider)
        {
            _accountBalanceService = accountBalanceService;
            _dataContext = dataContext;
            _userIdentityProvider = userIdentityProvider;
        }

        public MetaDataViewModel GetMetaData()
        {
            var accountIds = _dataContext.Accounts.Select(account => account.Id).ToList();
            var balance = accountIds.Sum(id => _accountBalanceService.GetAccountBalance(id, DateTime.Now));
            var targetDate = DateTime.Now.AddDays(-7);

            return new MetaDataViewModel
            {
                Username = _userIdentityProvider?.DisplayName ?? "Unknown user",
                Total = $"{balance:0.00}",
                AccountsNeedUpdating = _dataContext.Accounts.Any(x => x.LastUpdated < targetDate)
            };
        }
    }
}
