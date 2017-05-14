using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.ModelledTransactions
{
    public class ModelledTransactionAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ModelledTransactionViewModel, ModelledTransaction>().ReverseMap();
        }
    }
}