using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Reports
{
    public class ReportAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ReportViewModel, Report>();
            CreateMap<Report, ReportViewModel>();
        }
    }
}