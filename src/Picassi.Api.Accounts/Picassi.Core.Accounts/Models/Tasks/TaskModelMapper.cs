using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Tasks
{
    public interface IAssignmentRuleModelMapper : IModelMapper<TaskModel, Task>
    {

    }

    public class TaskModelMapper : IAssignmentRuleModelMapper
    {
        public Task CreateEntity(TaskModel model)
        {
            return new Task
            {
                Description = model.Description,
                Completed = model.Completed,
                CompletedDate = model.CompletedDate,
                CreatedDate = model.CreatedDate,
                DueDate = model.DueDate,
                Ordinal = model.Ordinal,
                Priority = model.Priority
            };
        }

        public TaskModel Map(Task model)
        {
            return new TaskModel
            {
                Id = model.Id,
                Description = model.Description,
                Completed = model.Completed,
                CompletedDate = model.CompletedDate,
                CreatedDate = DateTime.Now,
                DueDate = model.DueDate,
                Ordinal = model.Ordinal,
                Priority = model.Priority
            };
        }

        public void Patch(TaskModel model, Task entity)
        {
            if (model.Completed && !entity.Completed)
            {
                entity.CompletedDate = DateTime.UtcNow;
            }

            entity.Description = model.Description;
            entity.Completed = model.Completed;
            entity.DueDate = model.DueDate;
            entity.Ordinal = model.Ordinal;
            entity.Priority = model.Priority;
        }

        public IEnumerable<TaskModel> MapList(IEnumerable<Task> results)
        {
            return results.Select(Map);
        }
    }
}
