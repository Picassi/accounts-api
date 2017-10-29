using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Models.Tasks
{
    public class TasksQueryModel
    {
        public string Text { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string SortBy { get; set; }
        public bool? SortAscending { get; set; }
        public IList<TaskPriority> IncludePriorities { get; set; }
    }
}
