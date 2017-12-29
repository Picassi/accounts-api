using System;
using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class TransactionCategoriesGroupedByPeriodModel
    {
        public IEnumerable<TransactionsGroupedByCategoryModel> Transactions { get; set; }

        public string Description { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public decimal Amount => Credit + Debit;

        public decimal StartBalance { get; set; }

        public decimal EndBalance { get; set; }

        public DateTime StartDate { get; set; }
    }
}