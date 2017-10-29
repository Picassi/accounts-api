using System;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Note : IEntity
    {
        public int Id { get; set; }

        public string Headline { get; set; }

        public string Text { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}