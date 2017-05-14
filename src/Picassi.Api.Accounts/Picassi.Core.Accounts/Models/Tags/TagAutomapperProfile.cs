using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TagModel, Tag>();
            CreateMap<Tag, TagModel>();
        }
    }
}