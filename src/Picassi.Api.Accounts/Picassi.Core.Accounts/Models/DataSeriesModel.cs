using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models
{
    public class DataSeriesModel
    {
        public string Name { get; set; }
        public IList<object[]> Data { get; set; }
    }
}