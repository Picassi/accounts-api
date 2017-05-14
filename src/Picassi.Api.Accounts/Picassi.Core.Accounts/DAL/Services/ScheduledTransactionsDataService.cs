using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.ScheduledTransactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
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
