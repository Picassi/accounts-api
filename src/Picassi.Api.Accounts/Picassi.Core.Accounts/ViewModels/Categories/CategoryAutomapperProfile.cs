using AutoMapper;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategoryAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CategoryViewModel, Category>();
            CreateMap<Category, CategoryViewModel>();
        }
    }
}