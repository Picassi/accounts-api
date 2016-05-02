using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.ReportGroups
{
    public class ReportGroupViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
