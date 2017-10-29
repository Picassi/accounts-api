﻿using System;
using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Models.Tasks
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool Completed { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int Ordinal { get; set; }

        public TaskPriority Priority { get; set; }
    }
}
