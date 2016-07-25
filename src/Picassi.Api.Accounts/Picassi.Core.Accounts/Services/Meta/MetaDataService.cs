using System;
using System.Linq;
using System.Web;
using Picassi.Common.Data.Connection;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.ViewModels.Meta;
using Picassi.Data.Accounts.Database;

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

        public MetaDataService(IAccountBalanceService accountBalanceService, IAccountsDataContext dataContext)
        {
            _accountBalanceService = accountBalanceService;
            _dataContext = dataContext;
        }

        public MetaDataViewModel GetMetaData()
        {
            var accountIds = _dataContext.Accounts.Select(account => account.Id).ToList();
            var balance = accountIds.Sum(id => _accountBalanceService.GetAccountBalance(id, DateTime.Now));

            return new MetaDataViewModel
            {
                Username = "User",
                Total = $"{balance:0.00}"
            };
        }
    }
}
