using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.ScheduledTransactions
{
    public class ScheduledTransactionAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ScheduledTransactionModel, ModelledTransaction>().ReverseMap();
        }
    }
}