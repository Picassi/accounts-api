using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Reports
{
    public class ReportDefinitionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ReportType { get; set; }

        public string ReportSource { get; set; }

        public List<ReportGroupViewModel> Groups { get; set; }
    }

    public class ReportGroupViewModel
    {
        public int? Id { get; set; }        

        public string Name { get; set; }

        public List<ReportGroupCategoryViewModel> Categories { get; set; } 
    }

    public class ReportGroupCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

}
