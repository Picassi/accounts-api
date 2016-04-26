using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Groups;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Groups
{
    public interface IGroupCrudService
    {
        GroupViewModel CreateGroup(GroupViewModel group);
        GroupViewModel GetGroup(int id);
        GroupViewModel UpdateGroup(int id, GroupViewModel group);
        bool DeleteGroup(int id);
    }

    public class GroupCrudService : IGroupCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public GroupCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public GroupViewModel CreateGroup(GroupViewModel group)
        {
            var dataModel = Mapper.Map<Group>(group);
            _dbContext.Groups.Add(dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<GroupViewModel>(dataModel);
        }

        public GroupViewModel GetGroup(int id)
        {
            var dataModel = _dbContext.Groups.Find(id);
            return Mapper.Map<GroupViewModel>(dataModel);
        }

        public GroupViewModel UpdateGroup(int id, GroupViewModel group)
        {
            var dataModel = _dbContext.Groups.Find(id);
            Mapper.Map(group, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<GroupViewModel>(dataModel);
        }

        public bool DeleteGroup(int id)
        {
            var dataModel = _dbContext.Groups.Find(id);
            _dbContext.Groups.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
