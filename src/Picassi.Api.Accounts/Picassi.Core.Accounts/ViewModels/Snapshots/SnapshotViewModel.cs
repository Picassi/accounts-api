using System;

namespace Picassi.Core.Accounts.ViewModels.Snapshots
{
    public class SnapshotViewModel
    {
		public int Id { get; set; }
        public int AccountId { get; set; }
		public DateTime Date { get; set; }
		public decimal Amount { get; set; }
	}
}