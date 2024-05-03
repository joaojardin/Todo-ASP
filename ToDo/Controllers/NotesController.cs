using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Controllers
{
    public class NotesController : Controller
    {
        private readonly ToDoContext _context;
        private readonly INoteService _noteService;

        public NotesController(ToDoContext context, INoteService noteService)
        {
            _context = context;
            _noteService = noteService;

        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            return View(await _noteService.GetNotes());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Note == null)
            {
                return NotFound();
            }

            var note = await _noteService.GetNote((Guid)id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Date,Completed,Category,Priority")] Note note)
        {
            if (ModelState.IsValid)
            {
                bool result = await _noteService.CreateNote(note);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle the error appropriately
                    return View("Error");
                }
            }
            return View(note); // Return the view with the invalid model
        }


        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var note = await _noteService.GetNote((Guid)id);
            if (id == null || note == null)
            {
                return NotFound();
            }
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content,Date,Completed,Category,Priority")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }
            bool result = await _noteService.EditNote(note);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(note);
            }
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var note = await _noteService.GetNote((Guid)id);
            if (id == null || note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Note == null)
            {
                return Problem("Entity set 'ToDoContext.Note'  is null.");
            }
            var note = await _context.Note.FindAsync(id);
            if (note != null)
            {
                _context.Note.Remove(note);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            bool result = await _noteService.MarkNoteAsCompeleted(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Error");
            }
        }

        private bool NoteExists(Guid id)
        {
          return (_context.Note?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
