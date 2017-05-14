using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Budgets
{
    public class BudgetAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<BudgetModel, Budget>();
            CreateMap<Budget, BudgetModel>();
        }
    }
}