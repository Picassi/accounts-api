using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Extensions;
using Picassi.Core.Accounts.Models.Tasks;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ITasksDataService : IGenericDataService<TaskModel>
    {
        IEnumerable<TaskModel> Query(int pageNumber, int pageSize, bool showUncompleted = true,
            bool showCompleted = false, string sortBy = "CreatedDate", bool sortAscending = true,
            IList<TaskPriority> includePriorities = null);
    }

    public class TasksDataService : GenericDataService<TaskModel, Task>, ITasksDataService
    {
        public TasksDataService(IModelMapper<TaskModel, Task> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<TaskModel> Query(int pageNumber, int pageSize, bool showUncompleted = true, 
            bool showCompleted = false, string sortBy = null, bool sortAscending = true,
            IList<TaskPriority> includePriorities = null)
        {
            var queryResults = DbProvider.GetDataContext().Tasks
                .Where(x => (x.Completed && showCompleted) || (!x.Completed && showUncompleted))
                .AsQueryable();

            if (includePriorities != null)
            {
                queryResults = queryResults.Where(t => includePriorities.Contains(t.Priority));
            }

            queryResults = sortBy != null ? queryResults.OrderBy(sortBy, sortAscending) : queryResults.OrderBy(x => x.CreatedDate);

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
