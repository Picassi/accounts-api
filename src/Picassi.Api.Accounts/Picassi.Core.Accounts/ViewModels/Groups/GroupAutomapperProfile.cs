using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Groups
{
    public class GroupAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<GroupViewModel, Group>();
            CreateMap<Group, GroupViewModel>();
        }
    }
}