using System.Collections.Generic;
using Picassi.Data.Accounts.Enums;
using Picassi.Utils.Data;

namespace Picassi.Data.Accounts.Models
{
    public class Report : IModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ReportType ReportType { get; set; }

        public ReportSource ReportSource { get; set; }

        public virtual ICollection<ReportGroupReport> ReportGroups { get; set; }
    }
}