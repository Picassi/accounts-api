using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Statements
{
    public class StatementViewModel
    {
        public int AccountId { get; set; }

        public decimal StartBalance { get; set; }

        public IEnumerable<StatementLineViewModel> Lines { get; set; }
        
    }
}
