using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Notes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]    
    public class NotesController : ApiController
	{
        private readonly INotesDataService _dataService;

	    public NotesController(INotesDataService dataService)
	    {
	        _dataService = dataService;
	    }

	    [HttpGet]
        [Route("notes")]
        public IEnumerable<NoteModel> GetNotes([FromUri]NotesQueryModel query)
        {
            return _dataService.Query(query?.PageNumber ?? 1, query?.PageSize ?? 20).ToList();
        }

        [HttpPost]
        [Route("notes")]
        public NoteModel CreateNote([FromBody]NoteModel noteModel)
        {
            return _dataService.Create(noteModel);
        }

        [HttpGet]
        [Route("notes/{id}")]
        public NoteModel GetNote(int id)
        {
            return _dataService.Get(id);            
        }

	    [HttpPut]
        [Route("notes/{id}")]
        public NoteModel UpdateNote(int id, [FromBody]NoteModel noteModel)
        {
            return _dataService.Update(id, noteModel);
        }

        [HttpDelete]
        [Route("notes/{id}")]
        public bool DeleteNote(int id)
        {
            return _dataService.Delete(id);
        }
    }
}