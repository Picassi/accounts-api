using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagModelMapper : IModelMapper<TagModel, Tag>
    {
        public Tag CreateEntity(TagModel model)
        {
            return Mapper.Map<Tag>(model);
        }

        public TagModel Map(Tag model)
        {
            return Mapper.Map<TagModel>(model);
        }

        public void Patch(TagModel model, Tag entity)
        {
            Mapper.Map(model, entity);
        }
    }
}