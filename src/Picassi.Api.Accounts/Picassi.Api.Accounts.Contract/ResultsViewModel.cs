using System.Collections.Generic;

namespace Picassi.Api.Accounts.Contract
{
    public class ResultsViewModel<T>
    {
        public IEnumerable<T> Lines { get; set; }
        public int TotalLines { get; set; }
    }
}
