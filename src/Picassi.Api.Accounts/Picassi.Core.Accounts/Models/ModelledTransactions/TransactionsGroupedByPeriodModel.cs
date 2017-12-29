using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Services;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class TransactionsGroupedByPeriodModel
    {
        public DateTime StartDate { get; set; }

        public IList<ProjectedTransaction> Transactions { get; set; }
    }
}