using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IDebitOrCreditFilter
    {
        IQueryable<Transaction> GetDebits(DateTime from, DateTime to, IEnumerable<int> accounts);
        IQueryable<Transaction> GetCredits(DateTime from, DateTime to, IEnumerable<int> acounts);
    }

    public class DebitOrCreditFilter : IDebitOrCreditFilter
    {
        private readonly IAccountsDataContext _dbContext;

        public DebitOrCreditFilter(IAccountsDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Transaction> GetDebits(DateTime from, DateTime to, IEnumerable<int> accounts)
        {
            var debits = GetTransactionsForRange(from, to);
            return FilterDebits(debits, accounts);
        }

        public IQueryable<Transaction> GetCredits(DateTime from, DateTime to, IEnumerable<int> accounts)
        {
            var credits = GetTransactionsForRange(from, to);
            return FilterCredits(credits, accounts);
        }

        private IQueryable<Transaction> GetTransactionsForRange(DateTime from, DateTime to)
        {
            return _dbContext.Transactions.Where(x => x.Date >= from && x.Date < to);
        }

        private static IQueryable<Transaction> FilterDebits(IQueryable<Transaction> matchingDebits, IEnumerable<int> accounts)
        {
            var ret = matchingDebits.Where(x => x.FromId != null);
            if (accounts != null)
            {
                ret =  ret.Where(x => accounts.Contains((int)x.FromId));
            }
            return ret;
        }

        private static IQueryable<Transaction> FilterCredits(IQueryable<Transaction> matchingDebits, IEnumerable<int> accounts)
        {
            var ret = matchingDebits.Where(x => x.ToId != null);
            if (accounts != null)
            {
                ret = ret.Where(x => accounts.Contains((int)x.ToId));                
            }
            return ret;
        }
    }
}
