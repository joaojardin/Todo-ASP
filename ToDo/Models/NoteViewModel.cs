namespace ToDo.Models
{
    public class NoteViewModel
    {
        public IEnumerable<Note> Notes { get; set; }
        public Note NewNote { get; set; } = new Note();
    }

}
