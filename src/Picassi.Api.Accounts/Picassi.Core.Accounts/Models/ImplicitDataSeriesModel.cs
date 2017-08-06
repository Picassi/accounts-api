using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models
{
    public class ImplicitDataSeriesModel
    {
        public string Name { get; set; }
        public IList<object[]> Data { get; set; }
    }

    public class DataSeriesModel
    {
        public string Name { get; set; }
        public IList<DataPointModel> Data { get; set; }
        public bool ColorByPoint { get; set; }
    }

    public class DataPointModel
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
    }
}