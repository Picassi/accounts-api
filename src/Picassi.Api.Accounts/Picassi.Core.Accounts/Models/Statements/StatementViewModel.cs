using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models.Statements
{
    public class StatementViewModel
    {
        public int AccountId { get; set; }

        public decimal StartBalance { get; set; }

        public int TotalLines { get; set; }

        public IEnumerable<StatementLineViewModel> Lines { get; set; }
        
    }
}
