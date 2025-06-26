// schoolmanagement/Controllers/StudentsController.cs

// --- All 'using' statements go at the very top ---
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // <-- IMPORTANT: Add this for SelectList
using Microsoft.EntityFrameworkCore;
using schoolmanagement.Data;
using schoolmanagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace schoolmanagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Students (List all students)
        public async Task<IActionResult> Index()
        {
            // For Index, you might also want to include the Teacher to display their name
            return View(await _context.Students.Include(s => s.Teacher).ToListAsync());
        }

        // GET: Students/Details/5 (Show details of a specific student)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Teacher) // Eager load the Teacher data
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create (Display the form to create a new student)
        public IActionResult Create()
        {
            // --- ADDED THIS LINE: Populate the ViewBag with Teachers for the dropdown ---
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            return View();
        }

        // POST: Students/Create (Process the submitted form to create a new student)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DateOfBirth,Gender,PhoneNumber,TeacherId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // --- ADDED THIS LINE: Re-populate ViewBag if model is invalid ---
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // GET: Students/Edit/5 (Display the form to edit an existing student)
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
            // --- ADDED THIS LINE: Populate ViewBag for Edit dropdown ---
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // POST: Students/Edit/5 (Process the submitted form to update an existing student)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Gender,PhoneNumber,TeacherId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

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
            // --- ADDED THIS LINE: Re-populate ViewBag if model is invalid during Edit ---
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", student.TeacherId);
            return View(student);
        }

        // GET: Students/Delete/5 (Display confirmation page for deleting a student)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Teacher) // Include Teacher for display on Delete confirmation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5 (Perform the deletion)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
