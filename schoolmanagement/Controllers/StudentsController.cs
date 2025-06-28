using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using schoolmanagement.Data;
using schoolmanagement.Models;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize] attribute

namespace schoolmanagement.Controllers
{
    // Authorize attribute at controller level to ensure all actions require authentication.
    // By default, if no roles are specified, any authenticated user can access.
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Students
        // Any authenticated user can view the list of students.
        public async Task<IActionResult> Index()
        {
            // Eager load related Class and Teacher data to display their names
            var schoolDbContext = _context.Students.Include(s => s.Class).Include(s => s.Teacher);
            return View(await schoolDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        // Any authenticated user can view student details.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        // Only Admin users can access the Create page.
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Populate dropdowns for Class and Teacher for the form
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // Only Admin users can create new students.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,ClassId,TeacherId")] Student student)
        {
            // Remove Class and Teacher from ModelState validation to avoid circular reference or validation issues with navigation properties
            ModelState.Remove("Class");
            ModelState.Remove("Teacher");

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If model state is not valid, re-populate dropdowns and return view
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // GET: Students/Edit/5
        // Only Admin users can access the Edit page.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            // Populate dropdowns for Class and Teacher for the form
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // POST: Students/Edit/5
        // Only Admin users can save edits to students.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,ClassId,TeacherId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            // Remove Class and Teacher from ModelState validation
            ModelState.Remove("Class");
            ModelState.Remove("Teacher");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // If model state is not valid, re-populate dropdowns and return view
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // GET: Students/Delete/5
        // Only Admin users can access the Delete confirmation page.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        // Only Admin users can delete students.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'SchoolDbContext.Students' is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
