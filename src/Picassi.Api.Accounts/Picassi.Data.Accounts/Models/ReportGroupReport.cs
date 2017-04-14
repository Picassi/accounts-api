using Picassi.Utils.Data;

namespace Picassi.Data.Accounts.Models
{
    public class ReportGroupReport : IModel
    {
        public int Id { get; set; }

        public int Ordinal { get; set; }

        public int ReportGroupId { get; set; }

        public int ReportId { get; set; }

        public virtual ReportGroup ReportGroup { get; set; }

        public virtual Report Report { get; set; }
    }
}