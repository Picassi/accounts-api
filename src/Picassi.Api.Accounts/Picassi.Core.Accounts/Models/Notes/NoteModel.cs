using System;

namespace Picassi.Core.Accounts.Models.Notes
{
    public class NoteModel
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Headline { get; set; }

        public string Text { get; set; }
    }
}
