using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using schoolmanagement.Data; // Essential for SchoolDbContext
using schoolmanagement.Models; // Essential for your Student, Class, Teacher models

namespace schoolmanagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Students
        // This action fetches ALL students, including their related Class and Teacher entities.
        public async Task<IActionResult> Index()
        {
            // The .Include() calls are crucial here. They tell Entity Framework Core
            // to load the related 'Class' and 'Teacher' data along with each 'Student'.
            // Without .Include(), Class and Teacher navigation properties would be null,
            // even if ClassId and TeacherId foreign keys have values.
            var students = await _context.Students
                                        .Include(s => s.Class)    // Eager load the associated Class
                                        .Include(s => s.Teacher)  // Eager load the associated Teacher
                                        .ToListAsync();           // Execute the query and get all results

            // Pass the list of students (now including their Class and Teacher objects) to the view.
            return View(students);
        }

        // GET: Students/Details/5
        // Displays detailed information for a specific student.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the specific student, including their Class and Teacher.
            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id); // Find by primary key (Id)
            
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        // Displays the form to create a new student.
        public IActionResult Create()
        {
            // Populate SelectLists for dropdowns (Class and Teacher) in the create form.
            // "ClassId" and "Id" are the value properties, "Name" is the text displayed in the dropdown.
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // Processes the form submission for creating a new student.
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against Cross-Site Request Forgery (CSRF)
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,ClassId,TeacherId")] Student student)
        {
            // Validate the model based on data annotations.
            if (ModelState.IsValid)
            {
                _context.Add(student); // Add the new student to the context.
                await _context.SaveChangesAsync(); // Save changes to the database.
                return RedirectToAction(nameof(Index)); // Redirect back to the Students list.
            }
            // If model state is invalid, re-populate dropdowns and return the view with errors.
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // GET: Students/Edit/5
        // Displays the form to edit an existing student.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id); // Find student by ID.
            if (student == null)
            {
                return NotFound();
            }
            // Populate SelectLists for dropdowns in the edit form.
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // POST: Students/Edit/5
        // Processes the form submission for editing a student.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,ClassId,TeacherId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student); // Mark student for update.
                    await _context.SaveChangesAsync(); // Save changes.
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency issues.
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
                return RedirectToAction(nameof(Index)); // Redirect to the Students list.
            }
            // If model state is invalid, re-populate dropdowns and return the view with errors.
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // GET: Students/Delete/5
        // Displays a confirmation page before deleting a student.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include Class and Teacher data for display on the delete confirmation page.
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
        // Processes the deletion of a student.
        [HttpPost, ActionName("Delete")] // ActionName specifies which action this POST method corresponds to.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id); // Find the student to delete.
            if (student != null)
            {
                _context.Students.Remove(student); // Mark student for removal.
                await _context.SaveChangesAsync(); // Save changes to the database.
            }
            return RedirectToAction(nameof(Index)); // Redirect back to the Students list.
        }

        // Helper method to check if a student exists by ID.
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
