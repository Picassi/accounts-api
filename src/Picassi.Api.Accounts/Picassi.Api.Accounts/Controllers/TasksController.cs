using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Tasks;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TasksController : ApiController
    {
        private readonly ITasksDataService _dataService;

        public TasksController(ITasksDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("tasks")]
        public IEnumerable<TaskModel> GetTasks([FromUri]TasksQueryModel query)
        {
            return _dataService.Query(query?.PageNumber ?? 1, query?.PageSize ?? 20).ToList();
        }

        [HttpPost]
        [Route("tasks")]
        public TaskModel CreateTask([FromBody]TaskModel taskModel)
        {
            return _dataService.Create(taskModel);
        }

        [HttpGet]
        [Route("tasks/{id}")]
        public TaskModel GetTask(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("tasks/{id}")]
        public TaskModel UpdateTask(int id, [FromBody]TaskModel taskModel)
        {
            return _dataService.Update(id, taskModel);
        }

        [HttpDelete]
        [Route("tasks/{id}")]
        public bool DeleteTask(int id)
        {
            return _dataService.Delete(id);
        }
    }
}