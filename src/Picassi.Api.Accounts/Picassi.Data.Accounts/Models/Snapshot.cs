using System;

namespace Picassi.Data.Accounts.Models
{
    public class Snapshot
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public Account Account { get; set; }
    }
}
