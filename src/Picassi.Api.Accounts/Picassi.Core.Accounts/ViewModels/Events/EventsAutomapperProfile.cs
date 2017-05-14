using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Events
{
    public class EventAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<EventModel, Event>().ReverseMap();
        }
    }
}