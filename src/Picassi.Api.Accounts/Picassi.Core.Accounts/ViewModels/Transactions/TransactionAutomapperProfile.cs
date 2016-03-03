using System;
using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Transactions
{
    public class TransactionAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TransactionViewModel, Transaction>()
                .ForMember(x => x.Status,
                    options => options.MapFrom(x => (TransactionStatus)Enum.Parse(typeof (TransactionStatus), x.Status)));
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(x => x.CategoryName, options => options.MapFrom(x => x.Category == null ? null : x.Category.Name))
                .ForMember(x => x.Status, options => options.MapFrom(x => x.Status.ToString()));
        }
    }
}