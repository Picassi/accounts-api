using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagModelMapper : IModelMapper<TagModel, Tag>
    {
        public Tag CreateEntity(TagModel model)
        {
            return new Tag
            {
                Id = model.Id,
                Name = model.Name,
            };
        }

        public TagModel Map(Tag model)
        {
            return new TagModel
            {
                Id = model.Id,
                Name = model.Name,
            };
        }

        public void Patch(TagModel model, Tag entity)
        {
            entity.Name = model.Name;
        }

        public IEnumerable<TagModel> MapList(IEnumerable<Tag> results)
        {
            return results.Select(Map);
        }
    }
}