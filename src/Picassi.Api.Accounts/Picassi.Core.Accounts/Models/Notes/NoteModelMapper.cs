using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Notes
{
    public interface INoteModelMapper : IModelMapper<NoteModel, Note>
    {

    }

    public class NoteModelMapper : INoteModelMapper
    {
        public Note CreateEntity(NoteModel model)
        {
            return new Note
            {
                Headline = model.Headline,
                Text = model.Text,
                CreatedDate = DateTime.UtcNow
            };
        }

        public NoteModel Map(Note model)
        {
            return new NoteModel
            {
                Id = model.Id,
                Headline = model.Headline,
                Text = model.Text,
                CreatedDate = model.CreatedDate,
            };
        }

        public void Patch(NoteModel model, Note entity)
        {
            entity.Text = model.Text;
            entity.Headline = model.Headline;        
        }

        public IEnumerable<NoteModel> MapList(IEnumerable<Note> results)
        {
            return results.Select(Map);
        }
    }
}
