using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.ScheduledTransactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IScheduledTransactionsDataService : IGenericDataService<ScheduledTransactionModel>
    {
        IEnumerable<ScheduledTransactionModel> Query(ScheduledTransactionQueryModel query);
    }

    public class ScheduledTransactionsDataService : GenericDataService<ScheduledTransactionModel, ScheduledTransaction>, IScheduledTransactionsDataService
    {
        public ScheduledTransactionsDataService(IModelMapper<ScheduledTransactionModel, ScheduledTransaction> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<ScheduledTransactionModel> Query(ScheduledTransactionQueryModel query)
        {
            var queryResults = DbContext.ScheduledTransactions.Include("Category").AsQueryable();

            return queryResults.ToList().Select(ModelMapper.Map);
        }

    }
}
