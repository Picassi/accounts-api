using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Exceptions;
using Picassi.Core.Accounts.Models.Budgets;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IBudgetsDataService : IGenericDataService<BudgetModel>
    {
        IEnumerable<BudgetModel> Query(BudgetsQueryModel query);
    }

    public class BudgetsDataService : GenericDataService<BudgetModel, Budget>, IBudgetsDataService
    {
        public BudgetsDataService(IModelMapper<BudgetModel, Budget> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<BudgetModel> Query(BudgetsQueryModel query)
        {
            var queryResults = DbProvider.GetDataContext().Budgets
                .Include("Account").Include("Category").AsQueryable();

            return queryResults.ToList().Select(ModelMapper.Map);
        }

        public override BudgetModel Get(int id)
        {
            var entity = DbProvider.GetDataContext().Budgets
                .Include("Account")
                .Include("Category")
                .SingleOrDefault(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException<Transaction>(id);

            return ModelMapper.Map(entity);
        }


    }
}
