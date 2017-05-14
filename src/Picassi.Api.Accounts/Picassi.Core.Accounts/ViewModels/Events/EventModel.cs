using System;

namespace Picassi.Core.Accounts.ViewModels.Events
{
    public class EventModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? Date { get; set; }
    }
}
