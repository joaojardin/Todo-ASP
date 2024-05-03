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
            var success = 0;
            try
            {
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
            var note = await _context.Note.Where(i => i.id == nt.id).FirstOrDefaultAsync();
            var success = 0;
            try
            {
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

        public async Task<bool> DeleteNote(Guid id)
        {
            var note = await GetNote(id);
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


        public async Task<Note> GetNote(Guid id)
        {
            return await _context.Note.Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> MarkNoteAsCompeleted(Guid id)
        {
            var note = await GetNote(id);
            var success = 0;
            try
            {
                note.completed = true;
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
    }
}

