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
using NToastNotify;
using Microsoft.Extensions.Localization;
using ToDo.Resources;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToDo.Controllers
{
    public class NotesController : Controller
    {
        private readonly ToDoContext _context;
        private readonly INoteService _noteService;
        private readonly IToastNotification _toastNotification;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public NotesController(ToDoContext context, INoteService noteService, IToastNotification toastNotification, IStringLocalizer<SharedResource> localizer) 
        { 
            _context = context;
            _noteService = noteService;
            _toastNotification = toastNotification;
            _localizer = localizer;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = Enum.GetValues(typeof(NoteCategory));
            var notes = await _noteService.GetNotes();
            var viewModel = new NoteViewModel
            {
                Notes = notes,
                NewNote = new Note() // Assuming you want to initialize a new note for the form
            };

            return View(viewModel);
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
            ViewBag.Categories = Enum.GetValues(typeof(NoteCategory));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Date,Completed,Category,Priority")] Note note)
        {
            ViewBag.Categories = Enum.GetValues(typeof(NoteCategory));
            var notes = await _noteService.GetNotes();
            var viewModel = new NoteViewModel
            {
                Notes = notes,
                NewNote = new Note() // Assuming you want to initialize a new note for the form
            };

            if (ModelState.IsValid)
            {
                bool result = await _noteService.CreateNote(note);
                if (result)
                {
                    _toastNotification.AddSuccessToastMessage(note.Title + " created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Failed to create " + note.Title + ".");
                    return View("Index", viewModel);
                }
            }
            else
            {
                // Log errors
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        _toastNotification.AddErrorToastMessage(error.ErrorMessage);
                    }
                }
            }

            _toastNotification.AddErrorToastMessage("Validation failed.");
            return View("Index", viewModel);
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
            ViewBag.Categories = Enum.GetValues(typeof(NoteCategory));
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content,Date,Completed,Category,Priority")] Note note)
        {

            ViewBag.Categories = Enum.GetValues(typeof(NoteCategory));
            if (id != note.Id)
            {
                Console.WriteLine(id);
                Console.WriteLine(note.Id);
                Console.WriteLine("HEREEEEEEEEEEEEEEEEEEEEEEEEE");
                return NotFound();
            }
            bool result = await _noteService.EditNote(note);

            if (result)
            {
                _toastNotification.AddSuccessToastMessage(note.Title + " has been updated.");
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
            var note = await _noteService.GetNote(id);
            var success = await _noteService.DeleteNote(id);
            if (success)
            {
                _toastNotification.AddSuccessToastMessage(note.Title + " has been deleted.");

            }
            else
            {
                _toastNotification.AddErrorToastMessage("ERRORRR");
            }
            
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
