using System;

namespace Picassi.Core.Accounts.Models.Accounts
{
    public class AccountModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
	}
}