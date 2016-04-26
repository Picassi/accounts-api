using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Tags;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Tags
{
    public interface ITagQueryService
    {
        IEnumerable<TagViewModel> Query(TagsQueryModel accounts);
    }

    public class TagQueryService : ITagQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public TagQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<TagViewModel> Query(TagsQueryModel accounts)
        {			
            var queryResults = _dbContext.Tags.AsQueryable();

            if (accounts?.Name != null) queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));

            return Mapper.Map<IEnumerable<TagViewModel>>(queryResults);
        }
    }
}
