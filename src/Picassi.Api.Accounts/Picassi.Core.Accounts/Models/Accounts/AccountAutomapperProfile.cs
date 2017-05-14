using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Accounts
{
    public class AccountAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<AccountModel, Account>();
            CreateMap<Account, AccountModel>();
        }
    }
}