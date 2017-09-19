using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Services;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class TransactionCategoriesGroupedByPeriodModel
    {
        public IEnumerable<TransactionsGroupedByCategoryModel> Transactions { get; set; }

        public string Description { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public decimal Amount => Credit + Debit;

        public decimal Balance { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class TransactionsGroupedByPeriodModel
    {
        public DateTime StartDate { get; set; }

        public IList<ProjectedTransaction> Transactions { get; set; }
    }
}