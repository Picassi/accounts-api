using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Accounts
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