using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.ReportGroups
{
    public class ReportGroupAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ReportGroupViewModel, ReportGroup>();
            CreateMap<ReportGroup, ReportGroupViewModel>();
        }
    }
}