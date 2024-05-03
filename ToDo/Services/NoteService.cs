using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Services
{


    public class NoteService : INoteService
    {
        private ToDoContext _context;

        public NoteService(ToDoContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateNote(Note nt)
        {
            if (nt == null)
            {
                return false;
            }
            var success = 0;
            try
            {
                nt.Id = Guid.NewGuid();
                nt.Date = DateTime.Now;
                _context.Add(nt);
                success = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // https://learn.microsoft.com/en-us/ef/core/saving/concurrency#resolving-concurrency-conflicts
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }

            return success > 0;
        }

        public async Task<bool> EditNote(Note nt)
        {
            var note = await _context.Note.Where(i => i.Id == nt.Id).FirstOrDefaultAsync();
            var success = 0;
            try
            {
                note.Content = nt.Content;
                _context.Update(note);
                success = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // https://learn.microsoft.com/en-us/ef/core/saving/concurrency#resolving-concurrency-conflicts
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }

            return success > 0;
        }

        public async Task<bool> DeleteNote(Guid Id)
        {
            var note = await GetNote(Id);
            var success = 0;
            try
            {
                _context.Remove(note);
                success = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // https://learn.microsoft.com/en-us/ef/core/saving/concurrency#resolving-concurrency-conflicts
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }

            return success > 0;
        }


        public async Task<Note> GetNote(Guid Id)
        {
            return await _context.Note.Where(i => i.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> MarkNoteAsCompeleted(Guid Id)
        {
            var note = await GetNote(Id);
            var success = 0;
            try
            {
                note.Completed = true;
                success = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // https://learn.microsoft.com/en-us/ef/core/saving/concurrency#resolving-concurrency-conflicts
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error_ " + ex.Message);
                success = 0;
            }

            return success > 0;
        }

        public async Task<List<Note>> GetNotes()
        {
            return await _context.Note.ToListAsync();
        }
    }
}

