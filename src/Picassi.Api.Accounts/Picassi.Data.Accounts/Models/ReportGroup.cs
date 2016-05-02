using System.Collections.Generic;
using Picassi.Common.Data;

namespace Picassi.Data.Accounts.Models
{
    public class ReportGroup : IModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }        

        public virtual ICollection<ReportGroupReport> Reports { get; set; }

        public virtual ICollection<ReportGroupCategory> Categories { get; set; }
    }
}