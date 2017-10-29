using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Notes;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface INotesDataService : IGenericDataService<NoteModel>
    {
        IEnumerable<NoteModel> Query(int pageNumber, int pageSize);
    }

    public class NotesDataService : GenericDataService<NoteModel, Note>, INotesDataService
    {
        public NotesDataService(IModelMapper<NoteModel, Note> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<NoteModel> Query(int pageNumber, int pageSize)
        {
            var queryResults = DbProvider.GetDataContext().Notes.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
