using Microsoft.AspNetCore.Mvc;
using schoolmanagement.Data; // Ensure this matches your DbContext namespace
using Microsoft.AspNetCore.Authorization; // For authorization attributes
using System.Linq; // For .Count()

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
        public IActionResult Index()
        {
            // Retrieve counts of various entities from the database
            // .Count() is efficient as it translates to a COUNT(*) query in SQL
            var totalStudents = _context.Students.Count();
            var totalTeachers = _context.Teachers.Count();
            var totalClasses = _context.Classes.Count();
            var totalSubjects = _context.Subjects.Count();

            // Pass these counts to the View using ViewBag.
            // ViewBag is dynamic, making it easy to pass simple data to the view.
            ViewBag.TotalStudents = totalStudents;
            ViewBag.TotalTeachers = totalTeachers;
            ViewBag.TotalClasses = totalClasses;
            ViewBag.TotalSubjects = totalSubjects;

            // Return the default view associated with this action (Views/Dashboard/Index.cshtml)
            return View();
        }

        // You can add more dashboard-related actions here later if needed,
        // for example, specific charts or more detailed summaries.
    }
}