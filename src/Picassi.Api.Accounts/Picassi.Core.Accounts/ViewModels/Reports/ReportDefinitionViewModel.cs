using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Reports
{
    public class ReportDefinitionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ReportType { get; set; }

        public string ReportSource { get; set; }

        public List<int> GroupIds { get; set; }
    }
}
