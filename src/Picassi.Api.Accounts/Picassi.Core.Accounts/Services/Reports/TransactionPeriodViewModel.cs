using System.Linq;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public class TransactionPeriodViewModel
    {
        public int Identifier { get; set; }

        public IQueryable<Transaction> Credits { get; set; }

        public IQueryable<Transaction> Debits { get; set; }
    }
}
