using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Groups;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Groups
{
    public interface IGroupQueryService
    {
        IEnumerable<GroupViewModel> Query(GroupsQueryModel accounts);
    }

    public class GroupQueryService : IGroupQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public GroupQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<GroupViewModel> Query(GroupsQueryModel accounts)
        {			
            var queryResults = _dbContext.Groups.AsQueryable();

            if (accounts?.Name != null) queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));

            return Mapper.Map<IEnumerable<GroupViewModel>>(queryResults);
        }
    }
}
