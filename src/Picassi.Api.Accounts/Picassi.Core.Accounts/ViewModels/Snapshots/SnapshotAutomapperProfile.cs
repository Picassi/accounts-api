using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Snapshots
{
    public class SnapshotAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SnapshotViewModel, Snapshot>();
            CreateMap<Snapshot, SnapshotViewModel>();
        }
    }
}