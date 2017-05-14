using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.ScheduledTransactions
{
    public class ScheduledTransactionAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ScheduledTransactionViewModel, ModelledTransaction>().ReverseMap();
        }
    }
}