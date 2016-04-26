using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Tags
{
    public class TagAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TagViewModel, Tag>();
            CreateMap<Tag, TagViewModel>();
        }
    }
}