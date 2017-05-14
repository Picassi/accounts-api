using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.ModelledTransactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IModelledTransactionsDataService : IGenericDataService<ModelledTransactionViewModel>
    {
        IEnumerable<ModelledTransactionViewModel> Query(ModelledTransactionQueryModel query);
    }

    public class ModelledTransactionsDataService : GenericDataService<ModelledTransactionViewModel, ModelledTransaction>, IModelledTransactionsDataService
    {
        public ModelledTransactionsDataService(IModelMapper<ModelledTransactionViewModel, ModelledTransaction> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<ModelledTransactionViewModel> Query(ModelledTransactionQueryModel query)
        {
            var queryResults = DbContext.ModelledTransactions.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
