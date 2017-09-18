using System.Collections.Generic;
using System.Linq;
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
            return new Budget
            {
                AggregationPeriod = model.AggregationPeriod,
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                Period = model.Period,
                AccountId = model.AccountId
            };
        }

        public BudgetModel Map(Budget model)
        {
            return new BudgetModel
            {
                Id = model.Id,
                AggregationPeriod = model.AggregationPeriod,
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                Period = model.Period,
                AccountId = model.AccountId
            };
        }

        public void Patch(BudgetModel model, Budget entity)
        {
            entity.Amount = model.Amount;
            entity.Period = model.Period;
            entity.AggregationPeriod = model.AggregationPeriod;
            entity.AccountId = model.AccountId;
        }

        public IEnumerable<BudgetModel> MapList(IEnumerable<Budget> results)
        {
            return results.Select(Map);
        }
    }
}