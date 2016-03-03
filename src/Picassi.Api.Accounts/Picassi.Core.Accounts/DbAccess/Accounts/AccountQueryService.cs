using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Accounts
{
    public interface IAccountQueryService
    {
        IEnumerable<AccountViewModel> Query(AccountQueryModel accounts);
    }

    public class AccountQueryService : IAccountQueryService
    {
        private readonly IAccountsDataContext _dbContext;
		
        public AccountQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<AccountViewModel> Query(AccountQueryModel accounts)
        {			
            var queryResults = _dbContext.Accounts.AsQueryable();

            if (accounts?.Name != null)
            {
                queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));
            }

			return Mapper.Map<IEnumerable<AccountViewModel>>(queryResults);
        }
    }
}
