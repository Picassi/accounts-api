using System.Collections.Generic;

namespace Picassi.Data.Accounts.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<Category> Categories { get; set; }
    }
}