using AutoMapper;
using Picassi.Core.Accounts.Services;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Budgets
{
    public interface IBudgetModelMapper : IModelMapper<BudgetModel, Budget>
    {
        
            
    }

    public class BudgetModelMapper : IBudgetModelMapper
    {
        public Budget CreateEntity(BudgetModel model)
        {
            return Mapper.Map<Budget>(model);
        }

        public BudgetModel Map(Budget model)
        {
            return Mapper.Map<BudgetModel>(model);
        }

        public void Patch(BudgetModel model, Budget entity)
        {
            Mapper.Map(model, entity);
        }
    }
}