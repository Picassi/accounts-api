using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Snapshots;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Snapshots
{
    public interface ISnapshotQueryService
    {
        IEnumerable<SnapshotViewModel> Query(SnapshotQueryModel snapshots);
        SnapshotViewModel GetLastSnapshotBefore(int accountId, DateTime date);
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

        public SnapshotViewModel GetLastSnapshotBefore(int accountId, DateTime date)
        {
            var snapshotsBeforeDate = _dbContext.Snapshots.Where(x => x.AccountId == accountId && x.Date < date);
            var lastSnapshotBeforeDate = snapshotsBeforeDate.OrderByDescending(x => x.Date).FirstOrDefault();
            return lastSnapshotBeforeDate != null ? Mapper.Map<SnapshotViewModel>(lastSnapshotBeforeDate) : null;
        }
    }
}
