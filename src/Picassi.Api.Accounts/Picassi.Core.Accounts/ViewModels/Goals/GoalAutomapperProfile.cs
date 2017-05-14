using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Goals
{
    public class GoalAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<GoalModel, Goal>().ReverseMap();
        }
    }
}