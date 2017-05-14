using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.ScheduledTransactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
{
    public interface IScheduledTransactionsDataService : IGenericDataService<ScheduledTransactionViewModel>
    {
        IEnumerable<ScheduledTransactionViewModel> Query(ScheduledTransactionQueryModel query);
    }

    public class ScheduledTransactionsDataService : GenericDataService<ScheduledTransactionViewModel, ScheduledTransaction>, IScheduledTransactionsDataService
    {
        public ScheduledTransactionsDataService(IModelMapper<ScheduledTransactionViewModel, ScheduledTransaction> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<ScheduledTransactionViewModel> Query(ScheduledTransactionQueryModel query)
        {
            var queryResults = DbContext.ScheduledTransactions.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
