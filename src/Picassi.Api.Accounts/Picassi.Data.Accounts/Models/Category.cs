using System.Collections.Generic;

namespace Picassi.Data.Accounts.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public virtual IEnumerable<ReportGroupCategory> ReportGroups { get; set; }

    }
}