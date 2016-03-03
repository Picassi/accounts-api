using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Snapshots;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Snapshots
{
    public interface ISnapshotCrudService
    {
        SnapshotViewModel CreateSnapshot(SnapshotViewModel snapshot);
        SnapshotViewModel GetSnapshot(int id);
        SnapshotViewModel UpdateSnapshot(int id, SnapshotViewModel snapshot);
        bool DeleteSnapshot(int id);
    }

    public class SnapshotCrudService : ISnapshotCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public SnapshotCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public SnapshotViewModel CreateSnapshot(SnapshotViewModel snapshot)
        {
            var dataModel = Mapper.Map<Snapshot>(snapshot);
            _dbContext.Snapshots.Add(dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<SnapshotViewModel>(dataModel);
        }

        public SnapshotViewModel GetSnapshot(int id)
        {
            var dataModel = _dbContext.Snapshots.Find(id);
            return Mapper.Map<SnapshotViewModel>(dataModel);
        }

        public SnapshotViewModel UpdateSnapshot(int id, SnapshotViewModel snapshot)
        {
            var dataModel = _dbContext.Snapshots.Find(id);
            Mapper.Map(snapshot, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<SnapshotViewModel>(dataModel);
        }

        public bool DeleteSnapshot(int id)
        {
            var dataModel = _dbContext.Snapshots.Find(id);
            _dbContext.Snapshots.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
