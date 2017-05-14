using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Budgets
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