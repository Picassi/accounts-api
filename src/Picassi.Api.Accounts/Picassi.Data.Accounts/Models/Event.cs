using System;
using System.Collections.Generic;

namespace Picassi.Data.Accounts.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? Date { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Category Category { get; set; }
    }
}
