using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.ViewModels.Statements;
using Picassi.Core.Accounts.ViewModels.Transactions;

namespace Picassi.Core.Accounts.Reports
{
    public interface IStatementCompilerService
    {
        StatementViewModel CompileStatement(int accountId, decimal startBalance, IEnumerable<TransactionViewModel> transactions, string groupBy);
    }

    public class StatementViewModelFactory : IStatementCompilerService
    {
        public StatementViewModel CompileStatement(int accountId, decimal startBalance, IEnumerable<TransactionViewModel> transactions, string groupBy)
        {
            return new StatementViewModel { AccountId = accountId, StartBalance = startBalance, Lines = GetStatementLines(accountId, startBalance, transactions, groupBy) };
        }

        private IEnumerable<StatementLineViewModel> GetStatementLines(int accountId, decimal startBalance, IEnumerable<TransactionViewModel> transactions, string groupBy)
        {
            var cumulativeBalance = startBalance;
            var groupedTransactions = GroupTransactions(transactions, groupBy, accountId);
            return groupedTransactions
                .OrderBy(x => x.Date)
                .Select(transaction => GetStatementLine(transaction, accountId, cumulativeBalance, out cumulativeBalance))
                .ToList();
        }

        private IEnumerable<TransactionViewModel> GroupTransactions(IEnumerable<TransactionViewModel> transactions, string groupBy, int account)
        {
            switch (groupBy)
            {
                case "transaction":
                    return transactions;
                case "day":
                    return GetTransactionsByDay(transactions, account);
                case "week":
                    return GetTransactionsByWeek(transactions, account);
                case "month":
                    return GetTransactionsByMonth(transactions, account);
                case "year":
                    return GetTransactionsByYear(transactions, account);
                default:
                    return transactions;
            }
        }

        private static IEnumerable<TransactionViewModel> GetTransactionsByDay(IEnumerable<TransactionViewModel> transactions, int account)
        {
            return transactions
                .Select(x => AggregateByAccount(x, account))
                .GroupBy(x => new { x.ToId, x.FromId, Day = x.Date.Date })
                .Select(x => new TransactionViewModel
                {
                    Amount = x.Sum(y => y.Amount),
                    In = x.Where(y => y.Amount > 0).Sum(y => y.Amount),
                    Out = x.Where(y => y.Amount < 0).Sum(y => y.Amount),
                    Date = x.Key.Day,
                    Description = x.Key.Day.ToString("YYYY-MM-dd"),
                    FromId = x.Key.FromId,
                    ToId = x.Key.ToId
                }
            );
        }

        private static IEnumerable<TransactionViewModel> GetTransactionsByWeek(IEnumerable<TransactionViewModel> transactions, int account)
        {
            var comparisonDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);

            return transactions                       
                .Select(x => AggregateByAccount(x, account))
                .GroupBy(x => new { x.ToId, x.FromId, Week = (int)((x.Date - comparisonDate).TotalDays/7) })
                .Select(x => new TransactionViewModel
                {
                    Amount = x.Sum(y => y.Amount),
                    In = x.Where(y => y.Amount > 0).Sum(y => y.Amount),
                    Out = x.Where(y => y.Amount < 0).Sum(y => y.Amount),
                    Date = comparisonDate.AddDays(x.Key.Week * 7),
                    Description = "Week: " + x.Key.Week,
                    FromId = x.Key.FromId,
                    ToId = x.Key.ToId
                }
            );
        }

        private static IEnumerable<TransactionViewModel> GetTransactionsByMonth(IEnumerable<TransactionViewModel> transactions, int account)
        {
            return transactions
                .Select(x => AggregateByAccount(x, account))
                .GroupBy(x => new { x.ToId, x.FromId, x.Date.Month, x.Date.Year })
                .Select(x => new TransactionViewModel
                {
                    Amount = x.Sum(y => y.Amount),
                    In = x.Where(y => y.Amount > 0).Sum(y => y.Amount),
                    Out = x.Where(y => y.Amount < 0).Sum(y => y.Amount),
                    Date = new DateTime(x.Key.Year, x.Key.Month, 1),
                    Description = "Month " + x.Key.Month + "(" + x.Key.Year + ")",
                    FromId = x.Key.FromId,
                    ToId = x.Key.ToId
                }
            );
        }

        private static IEnumerable<TransactionViewModel> GetTransactionsByYear(IEnumerable<TransactionViewModel> transactions, int account)
        {
            return transactions
                .Select(x => AggregateByAccount(x, account))
                .GroupBy(x => new { x.ToId, x.FromId, Year = x.Date.Year })
                .Select(x => new TransactionViewModel
                {
                    Amount = x.Sum(y => y.Amount),
                    In = x.Where(y => y.Amount > 0).Sum(y => y.Amount),
                    Out = x.Where(y => y.Amount < 0).Sum(y => y.Amount),
                    Date = new DateTime(x.Key.Year, 1, 1),
                    Description = x.Key.Year.ToString(),
                    FromId = x.Key.FromId,
                    ToId = x.Key.ToId
                }
            );
        }


        private static TransactionViewModel AggregateByAccount(TransactionViewModel transaction, int account)
        {
            if (account == transaction.ToId)
            {
                return transaction;
            }

            return new TransactionViewModel
            {
                Amount = -transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                FromId = transaction.ToId,
                ToId = transaction.FromId
            };
        }

        private StatementLineViewModel GetStatementLine(TransactionViewModel transaction, int accountId, decimal balance, out decimal newBalance)
        {
            var transactionsValue = transaction.FromId == accountId ? -transaction.Amount : transaction.Amount;
            var amountIn = transaction.FromId == accountId ? transaction.Amount : 0;
            var amountOut = transaction.ToId == accountId ? transaction.Amount : 0;
            var linkedAccount = transaction.FromId == accountId ? transaction.ToId : transaction.FromId;
            newBalance = balance + transactionsValue;

            return new StatementLineViewModel
            {
                TransactionId = transaction.Id,
                LinkedAccountId = linkedAccount,
                Balance = newBalance,
                Date = transaction.Date,
                Description = transaction.Description,
                TransactionValue = transactionsValue,
                AmountIn = amountIn,
                AmountOut = amountOut,
            };
        }

    }
}
