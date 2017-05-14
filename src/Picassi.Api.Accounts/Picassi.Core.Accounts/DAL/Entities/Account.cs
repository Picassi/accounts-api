using System;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
