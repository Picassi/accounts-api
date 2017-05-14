using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.ModelledTransactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
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
