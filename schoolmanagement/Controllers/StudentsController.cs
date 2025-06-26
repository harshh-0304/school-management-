// schoolmanagement/Controllers/StudentsController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectList
using Microsoft.EntityFrameworkCore;
using schoolmanagement.Data;
using schoolmanagement.Models;

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
        public async Task<IActionResult> Index()
        {
            // Include both Teacher and Class navigation properties for display on the Index page
            var schoolDbContext = _context.Students
                                          .Include(s => s.Teacher)
                                          .Include(s => s.Class); // Added Class include
            return View(await schoolDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include both Teacher and Class navigation properties for details view
            var student = await _context.Students
                .Include(s => s.Teacher)
                .Include(s => s.Class) // Added Class include
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            // Populate the dropdown list for teachers
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            // Populate the dropdown list for classes
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ENSURE ClassId is included in the Bind attribute
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,TeacherId,ClassId")] Student student)
        {
            // ModelState.Remove("Teacher"); // Uncomment this line ONLY if you face issues with Teacher navigation property validation,
            // but typically it's not needed if TeacherId is nullable and handled correctly.
            // ModelState.Remove("Class"); // Uncomment this line ONLY if you face issues with Class navigation property validation

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate both dropdown lists if the model state is invalid and the view needs to be re-rendered
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId); // Repopulate Class dropdown
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            // Populate the dropdown list for teachers for the edit view
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            // Populate the dropdown list for classes for the edit view
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId); // Populate Class dropdown
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ENSURE ClassId is included in the Bind attribute
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,TeacherId,ClassId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            // ModelState.Remove("Teacher"); // Uncomment this line ONLY if you face issues with Teacher navigation property validation
            // ModelState.Remove("Class"); // Uncomment this line ONLY if you face issues with Class navigation property validation

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
            // Re-populate both dropdown lists if the model state is invalid
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", student.ClassId); // Repopulate Class dropdown
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Teacher)
                .Include(s => s.Class) // Added Class include for Delete view
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
            return _context.Students.Any(e => e.Id == id);
        }
    }
}