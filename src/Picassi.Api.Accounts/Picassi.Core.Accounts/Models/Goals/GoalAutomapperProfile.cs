using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Goals
{
    public class GoalAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<GoalModel, Goal>().ReverseMap();
        }
    }
}