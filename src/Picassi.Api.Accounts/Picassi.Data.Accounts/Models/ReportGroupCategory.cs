using Picassi.Common.Data;

namespace Picassi.Data.Accounts.Models
{
    public class ReportGroupCategory : IModel
    {
        public int Id { get; set; }

        public int Ordinal { get; set; }

        public int GroupId { get; set; }

        public int CategoryId { get; set; }

        public virtual ReportGroup ReportGroup { get; set; }

        public virtual Category Category { get; set; }
    }
}
