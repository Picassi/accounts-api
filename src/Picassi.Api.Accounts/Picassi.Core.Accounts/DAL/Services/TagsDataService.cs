using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Tags;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.Extensions;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ITagsDataService : IGenericDataService<TagModel>
    {
        IEnumerable<TagModel> Query(TagsQueryModel query);
    }

    public class TagsDataService : GenericDataService<TagModel, Tag>, ITagsDataService
    {
        public TagsDataService(IModelMapper<TagModel, Tag> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<TagModel> Query(TagsQueryModel query)
        {
            var queryResults = DbProvider.GetDataContext().Tags.AsQueryable();

            if (query?.Name != null) queryResults = queryResults.Where(x => x.Name.Contains(query.Name));
            queryResults = query == null ? queryResults : OrderResults(queryResults, query.SortBy, query.SortAscending);
            return ModelMapper.MapList(queryResults);
        }

        private static IQueryable<Tag> OrderResults(IQueryable<Tag> tags, string field, bool ascending)
        {
            if (field == null)
            {
                field = "Id";
                @ascending = true;
            }

            return field == "Id" ? tags.OrderBy(field, @ascending) : tags.OrderBy(field, @ascending).ThenBy("Id", @ascending);
        }

    }
}
