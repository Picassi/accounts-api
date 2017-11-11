using System.Threading.Tasks;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ITransactionBalanceAdjuster
    {
        Task UpdateStatementBalances(TransactionModel transaction);
    }

    public class TransactionBalanceAdjuster : ITransactionBalanceAdjuster
    {
        private readonly IAccountsDatabaseProvider _dbProvider;

        public TransactionBalanceAdjuster(IAccountsDatabaseProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task UpdateStatementBalances(TransactionModel transaction)
        {
            await UpdateBalancesBackwards(transaction.AccountId, transaction.StatementTransactionNumber, transaction.Balance);
            await UpdateBalancesForwards(transaction.AccountId, transaction.StatementTransactionNumber, transaction.Balance);            
        }

        private async Task<int> UpdateBalancesBackwards(int accountId, int statementTransactionNumber, decimal balance)
        {
            var query = $@"update t set t.Balance = 
                           (select @p0 - ISNULL(sum(Amount), 0) from accounts.Transactions tt 
                           where tt.AccountId = @p1
                           and tt.StatementTransactionNumber <= @p2
                           and tt.StatementTransactionNumber >= t.StatementTransactionNumber) 
                           from accounts.Transactions t where t.StatementTransactionNumber < @p2";

            var result = await _dbProvider.GetDataContext().ExecuteSqlCommand(query, balance, accountId, statementTransactionNumber);

            return result;
        }

        private async Task<int> UpdateBalancesForwards(int accountId, int statementTransactionNumber, decimal balance)
        {
            var query = $@"update t set t.Balance = 
                           (select ISNULL(sum(Amount), 0) + @p0 from accounts.Transactions tt 
                           where tt.AccountId = @p1
                           and tt.StatementTransactionNumber > @p2
                           and tt.StatementTransactionNumber <= t.StatementTransactionNumber) 
                           from accounts.Transactions t where t.StatementTransactionNumber >= @p2";

            var result = await _dbProvider.GetDataContext().ExecuteSqlCommand(query, balance, accountId, statementTransactionNumber);

            return result;
        }
    }
}
