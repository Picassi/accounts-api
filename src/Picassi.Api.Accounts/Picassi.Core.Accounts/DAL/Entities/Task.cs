using System;
using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Task : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int Ordinal { get; set; }

        public TaskPriority Priorty { get; set; }
    }
}