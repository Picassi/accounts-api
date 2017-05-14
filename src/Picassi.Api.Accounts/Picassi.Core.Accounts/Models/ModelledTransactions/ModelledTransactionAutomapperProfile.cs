using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class ModelledTransactionAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ModelledTransactionViewModel, ModelledTransaction>().ReverseMap();
        }
    }
}