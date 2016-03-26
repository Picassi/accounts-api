using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.ViewModels.Statements;
using Picassi.Core.Accounts.ViewModels.Transactions;

namespace Picassi.Core.Accounts.Reports
{
    public interface IStatementCompilerService
    {
        StatementViewModel CompileStatement(int accountId, decimal startBalance, IEnumerable<TransactionViewModel> transactions);
    }

    public class StatementViewModelFactory : IStatementCompilerService
    {
        public StatementViewModel CompileStatement(int accountId, decimal startBalance, IEnumerable<TransactionViewModel> transactions)
        {
            return new StatementViewModel { AccountId = accountId, StartBalance = startBalance, Lines = GetStatementLines(accountId, startBalance, transactions) };
        }

        private IEnumerable<StatementLineViewModel> GetStatementLines(int accountId, decimal startBalance, IEnumerable<TransactionViewModel> transactions)
        {
            var cumulativeBalance = startBalance;
            return transactions
                .OrderBy(x => x.Date)
                .Select(transaction => GetStatementLine(transaction, accountId, cumulativeBalance, out cumulativeBalance))
                .ToList();
        }

        private static StatementLineViewModel GetStatementLine(TransactionViewModel transaction, int accountId, decimal balance, out decimal newBalance)
        {
            var transactionsValue = transaction.FromId == accountId ? -transaction.Amount : transaction.Amount;
            var amountIn = transactionsValue > 0 ? transactionsValue : 0;
            var amountOut = transactionsValue < 0 ? -transactionsValue : 0;
            var linkedAccount = transaction.FromId == accountId ? transaction.ToId : transaction.FromId;
            newBalance = balance + transactionsValue;

            return new StatementLineViewModel
            {
                TransactionId = transaction.Id,
                LinkedAccountId = linkedAccount,
                Balance = newBalance,
                Date = transaction.Date,
                Description = transaction.Description,
                Amount = transactionsValue,
                Credit = amountIn,
                Debit = amountOut,
            };
        }

    }
}
