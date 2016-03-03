using System.Collections.Generic;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Snapshots;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Snapshots
{
    public interface ISnapshotQueryService
    {
        IEnumerable<SnapshotViewModel> Query(SnapshotQueryModel snapshots);
    }

    public class SnapshotQueryService : ISnapshotQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public SnapshotQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<SnapshotViewModel> Query(SnapshotQueryModel snapshots)
        {			
            var queryResults = _dbContext.Snapshots; 
			return Mapper.Map<IEnumerable<SnapshotViewModel>>(queryResults);
        }
    }
}
