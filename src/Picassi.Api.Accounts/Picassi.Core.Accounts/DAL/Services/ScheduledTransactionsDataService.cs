using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Exceptions;
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
        public ScheduledTransactionsDataService(IModelMapper<ScheduledTransactionModel, ScheduledTransaction> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<ScheduledTransactionModel> Query(ScheduledTransactionQueryModel query)
        {
            var queryResults = DbProvider.GetDataContext().ScheduledTransactions.Include("Category").AsQueryable();

            return queryResults.ToList().Select(ModelMapper.Map);
        }

        public override ScheduledTransactionModel Get(int id)
        {
            var entity = DbProvider.GetDataContext().ScheduledTransactions
                .Include("Account")
                .Include("To")
                .Include("Category")
                .SingleOrDefault(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException<Transaction>(id);

            return ModelMapper.Map(entity);
        }

    }
}
