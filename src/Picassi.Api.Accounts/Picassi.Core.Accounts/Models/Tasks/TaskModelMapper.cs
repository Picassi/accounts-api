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
                Description = model.Description            
            };
        }

        public TaskModel Map(Task model)
        {
            return new TaskModel
            {
                Id = model.Id,
                Description = model.Description
            };
        }

        public void Patch(TaskModel model, Task entity)
        {
            entity.Description = model.Description;
        }

        public IEnumerable<TaskModel> MapList(IEnumerable<Task> results)
        {
            return results.Select(Map);
        }
    }
}
