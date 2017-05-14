using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
{
    public interface IAccountDataService : IGenericDataService<AccountModel>
    {
        IEnumerable<AccountModel> Query(AccountQueryModel accounts);
    }

    public class AccountDataService : GenericDataService<AccountModel, Account>, IAccountDataService
    {
        public AccountDataService(IAccountModelMapper modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<AccountModel> Query(AccountQueryModel accounts)
        {
            var queryResults = DbContext.Accounts.AsQueryable();

            if (accounts?.Name != null)
            {
                queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));
            }

            return queryResults.Select(ModelMapper.Map);
        }
    }
}
