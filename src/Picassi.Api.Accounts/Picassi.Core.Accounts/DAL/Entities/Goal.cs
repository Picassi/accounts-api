using System.Collections.Generic;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Goal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
