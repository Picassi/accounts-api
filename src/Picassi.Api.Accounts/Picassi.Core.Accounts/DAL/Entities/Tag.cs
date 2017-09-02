using System.Collections.Generic;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; }

        public virtual ICollection<ModelledTransaction> ModelledTransactions { get; set; }

    }
}
