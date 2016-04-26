using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Tags;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Tags
{
    public interface ITagCrudService
    {
        TagViewModel CreateTag(TagViewModel tag);
        TagViewModel GetTag(int id);
        TagViewModel UpdateTag(int id, TagViewModel tag);
        bool DeleteTag(int id);
    }

    public class TagCrudService : ITagCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public TagCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public TagViewModel CreateTag(TagViewModel tag)
        {
            var dataModel = Mapper.Map<Tag>(tag);
            _dbContext.Tags.Add(dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<TagViewModel>(dataModel);
        }

        public TagViewModel GetTag(int id)
        {
            var dataModel = _dbContext.Tags.Find(id);
            return Mapper.Map<TagViewModel>(dataModel);
        }

        public TagViewModel UpdateTag(int id, TagViewModel tag)
        {
            var dataModel = _dbContext.Tags.Find(id);
            Mapper.Map(tag, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<TagViewModel>(dataModel);
        }

        public bool DeleteTag(int id)
        {
            var dataModel = _dbContext.Tags.Find(id);
            _dbContext.Tags.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
