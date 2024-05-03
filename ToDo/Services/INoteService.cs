using ToDo.Models;

namespace ToDo.Services
{
    public interface INoteService
    {
        public Task<bool> CreateNote(Note nt);
        public Task<bool> EditNote(Note nt);

        public Task<bool> DeleteNote(Guid id);

        public Task<bool> MarkNoteAsCompeleted(Guid id);
        public Task<Note> GetNote(Guid id);

        public Task<List<Note>> GetNotes();
    }
}
