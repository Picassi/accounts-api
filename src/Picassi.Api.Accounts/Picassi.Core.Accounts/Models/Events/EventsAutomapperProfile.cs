using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Events
{
    public class EventAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<EventModel, Event>().ReverseMap();
        }
    }
}