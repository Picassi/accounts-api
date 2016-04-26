using System.Collections.Generic;
using Picassi.Data.Accounts.Enums;

namespace Picassi.Data.Accounts.Models
{
    public class Report
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ReportType ReportType { get; set; }

        public ReportSource ReportSource { get; set; }

        public virtual IEnumerable<ReportLineDefinition> Lines { get; set; }
    }
}