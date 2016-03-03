using System;

namespace Picassi.Core.Accounts.ViewModels.Statements
{
    public class StatementLineViewModel
    {
        public int TransactionId { get; set; }

        public int? LinkedAccountId { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public decimal TransactionValue { get; set; }

        public decimal AmountIn { get; set; }

        public decimal AmountOut { get; set; }

        public decimal Balance { get; set; }
    }
}
