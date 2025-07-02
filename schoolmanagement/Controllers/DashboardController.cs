using Microsoft.AspNetCore.Mvc;
using schoolmanagement.Data; // This should be your DbContext's namespace
using Microsoft.AspNetCore.Authorization; // For authorization attributes (if you uncomment them)
using System.Linq; // For .Count() and .OrderByDescending()
using System.Threading.Tasks; // Required for async/await
using Microsoft.EntityFrameworkCore; // Crucial for ToListAsync() and CountAsync()

namespace schoolmanagement.Controllers
{
    // This controller will handle actions related to the dashboard.
    // [Authorize] // Optional: Uncomment this line if you want only authenticated users to access the dashboard
    // [Authorize(Roles = "Admin")] // Optional: Uncomment this line if you want ONLY Admin users to access the dashboard
    public class DashboardController : Controller
    {
        private readonly SchoolDbContext _context;

        // Constructor: Dependency Injection to get an instance of your database context
        public DashboardController(SchoolDbContext context)
        {
            _context = context;
        }

        // Action method for the Dashboard's main view (e.g., /Dashboard or /Dashboard/Index)
        public async Task<IActionResult> Index()
        {
            // Retrieve counts of various entities from the database asynchronously
            var totalStudents = await _context.Students.CountAsync();
            var totalTeachers = await _context.Teachers.CountAsync();
            var totalClasses = await _context.Classes.CountAsync();
            var totalSubjects = await _context.Subjects.CountAsync();

            // Fetch recent additions (e.g., last 5) asynchronously
            // Assuming your entities have an 'Id' property.
            // If your Class model uses 'ClassId' as its primary key, we use that.
            var recentStudents = await _context.Students
                                                .OrderByDescending(s => s.Id)
                                                .Take(5)
                                                .ToListAsync();

            var recentTeachers = await _context.Teachers
                                                .OrderByDescending(t => t.Id)
                                                .Take(5)
                                                .ToListAsync();

            var recentClasses = await _context.Classes
                                                .OrderByDescending(c => c.ClassId) // <--- CHANGED TO c.ClassId
                                                .Take(5)
                                                .ToListAsync();

            // Pass these counts and recent lists to the View using ViewBag.
            ViewBag.TotalStudents = totalStudents;
            ViewBag.TotalTeachers = totalTeachers;
            ViewBag.TotalClasses = totalClasses;
            ViewBag.TotalSubjects = totalSubjects;

            ViewBag.RecentStudents = recentStudents;
            ViewBag.RecentTeachers = recentTeachers;
            ViewBag.RecentClasses = recentClasses;

            // Return the default view associated with this action (Views/Dashboard/Index.cshtml)
            return View();

        }
 
    }
}