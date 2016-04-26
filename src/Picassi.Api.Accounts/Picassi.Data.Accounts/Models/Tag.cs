using System.Collections.Generic;

namespace Picassi.Data.Accounts.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }
    }
}