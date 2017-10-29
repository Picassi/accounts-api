using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Tasks;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ITasksDataService : IGenericDataService<TaskModel>
    {
        IEnumerable<TaskModel> Query(int pageNumber, int pageSize);
    }

    public class TasksDataService : GenericDataService<TaskModel, Task>, ITasksDataService
    {
        public TasksDataService(IModelMapper<TaskModel, Task> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<TaskModel> Query(int pageNumber, int pageSize)
        {
            var queryResults = DbProvider.GetDataContext().Tasks.Where(t => !t.Completed).AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
