using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Transactions
{
    public class TransactionAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TransactionModel, Transaction>();
            CreateMap<Transaction, TransactionModel>()
                .ForMember(x => x.CategoryName,
                    options => options.MapFrom(x => x.Category == null ? null : x.Category.Name));
        }
    }
}