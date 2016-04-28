namespace Picassi.Data.Accounts.Models
{
    public class ReportLineDefinition
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Ordinal { get; set; }

        public int ReportId { get; set; }

        public int? GroupId { get; set; }

        public int? CategoryId { get; set; }

        public int? TagId { get; set; }

        public virtual Report Report { get; set; }

        public virtual Group Group { get; set; }

        public virtual Category Category { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
