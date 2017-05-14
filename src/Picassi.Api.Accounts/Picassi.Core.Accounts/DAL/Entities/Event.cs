using System;
using System.Collections.Generic;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; }

        public virtual ICollection<ModelledTransaction> ModelledTransactions { get; set; }
    }
}
