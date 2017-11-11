using System.Threading.Tasks;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ITransactionSorter
    {
        Task UpdateStatementOrder(int accountId);
    }

    public class TransactionSorter : ITransactionSorter
    {
        private readonly IAccountsDatabaseProvider _dbProvider;

        public TransactionSorter(IAccountsDatabaseProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task UpdateStatementOrder(int accountId)
        {
            var query = $@"update t set t.StatementTransactionNumber = 
                           (select count(*) + 1 from accounts.Transactions tt
                           where tt.AccountId = @p0
                           and tt.Date < t.Date 
                           or (tt.Date = t.Date and tt.Ordinal < t.Ordinal)
                           or (tt.Date = t.Date and tt.Ordinal = t.Ordinal and tt.Id < t.Id))
                           from accounts.Transactions t";

            await _dbProvider.GetDataContext().ExecuteSqlCommand(query, accountId);
        }

    }
}
