using System;

namespace Picassi.Core.Accounts.Models.Accounts
{
    public class AccountPeriodViewModel
    {
        public int AccountId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
