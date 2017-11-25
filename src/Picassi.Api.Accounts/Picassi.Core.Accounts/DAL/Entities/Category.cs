using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public CategoryType CategoryType { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Budget> Budget { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; }

        public virtual ICollection<ModelledTransaction> ModelledTransactions { get; set; }

        public virtual ICollection<Category> Children { get; set; }

    }
}