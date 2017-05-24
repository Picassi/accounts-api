using System;

namespace Picassi.Api.Accounts.Contract.Events
{
    public class CreateEventJson
    {
        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? Date { get; set; }
    }
}
