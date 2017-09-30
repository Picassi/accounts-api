using System;
using System.Collections.Generic;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class AssignmentRule : IEntity
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string DescriptionRegex { get; set; }

        public bool Enabled { get; set; }

        public int? Priority { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
