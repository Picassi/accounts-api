using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IAccountDataService : IGenericDataService<AccountModel>
    {
        IEnumerable<AccountModel> Query(AccountQueryModel accounts);
    }

    public class AccountDataService : GenericDataService<AccountModel, Account>, IAccountDataService
    {
        public AccountDataService(IAccountModelMapper modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<AccountModel> Query(AccountQueryModel accounts)
        {
            var queryResults = DbProvider.GetDataContext().Accounts.AsQueryable();

            if (accounts?.Ids != null && accounts.Ids.Length > 0)
            {
                queryResults = queryResults.Where(x => accounts.Ids.Contains(x.Id));
            }

            if (accounts?.Name != null)
            {
                queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));
            }

            return queryResults.Select(ModelMapper.Map);
        }
    }
}
