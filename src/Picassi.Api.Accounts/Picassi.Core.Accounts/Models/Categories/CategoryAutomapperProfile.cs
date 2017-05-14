using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategoryAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();
        }
    }
}