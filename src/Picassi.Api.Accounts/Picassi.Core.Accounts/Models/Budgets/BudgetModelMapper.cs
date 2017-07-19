using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Budgets
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

        public IEnumerable<BudgetModel> MapList(IEnumerable<Budget> results)
        {
            return results.Select(Map);
        }
    }
}