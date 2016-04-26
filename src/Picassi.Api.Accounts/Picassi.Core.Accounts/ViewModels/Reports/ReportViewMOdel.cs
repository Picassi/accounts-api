using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Reports
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ReportType { get; set; }

        public string ReportSource { get; set; }

        public List<ReportLineDefinitionViewModel> LineDefinitions { get; set; } 
    }
}
