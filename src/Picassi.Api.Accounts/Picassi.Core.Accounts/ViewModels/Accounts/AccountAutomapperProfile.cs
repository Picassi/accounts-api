using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Accounts
{
    public class AccountAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<AccountViewModel, Account>();
            CreateMap<Account, AccountViewModel>();
        }
    }
}