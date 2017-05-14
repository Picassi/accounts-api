using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models
{
    public class ResultsViewModel<T>
    {
        public IEnumerable<T> Lines { get; set; }
        public int TotalLines { get; set; }
    }
}
