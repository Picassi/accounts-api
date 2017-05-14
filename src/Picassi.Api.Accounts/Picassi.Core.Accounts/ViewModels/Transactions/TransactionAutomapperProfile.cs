using System;
using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Transactions
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