    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity; // Required for UserManager/RoleManager
    using schoolmanagement.Models.Auth; // Required for ApplicationUser
    using schoolmanagement.ViewModels; // <--- THIS IS THE CRUCIAL USING DIRECTIVE
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore; // Required for .ToListAsync()

    namespace schoolmanagement.Controllers
    {
        // This attribute restricts access to this entire controller to users in the "Admin" role only.
        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            // Constructor to inject UserManager and RoleManager
            public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            // GET: /Admin/Index
            // Main entry point for the Admin Dashboard.
            public IActionResult Index()
            {
                ViewData["Title"] = "Admin Dashboard";
                return View();
            }

            // GET: /Admin/ManageUsers
            // This action will retrieve and display a list of all users and their roles.
            public async Task<IActionResult> ManageUsers()
            {
                ViewData["Title"] = "Manage Users";

                // Get all ApplicationUsers from the database
                var users = await _userManager.Users.ToListAsync();

                // Create a list to hold UserViewModel objects, which will be passed to the view
                var usersWithRoles = new List<UserViewModel>();

                // Iterate through each user to get their roles
                foreach (var user in users)
                {
                    // Get the roles assigned to the current user
                    var roles = await _userManager.GetRolesAsync(user);

                    // Create a new UserViewModel instance for the current user
                    usersWithRoles.Add(new UserViewModel
                    {
                        Id = user.Id, // User's unique ID
                        Email = user.Email, // User's email
                        Roles = roles.ToList() // List of roles (e.g., "Admin", "Teacher")
                    });
                }

                // Pass the populated list of usersWithRoles to the ManageUsers view
                return View(usersWithRoles);
            }

            // You can add more admin-specific actions here as needed for full user management,
            // such as:
            // [HttpGet]
            // public async Task<IActionResult> EditUserRoles(string id) { ... } // To edit a user's roles
            // [HttpPost]
            // public async Task<IActionResult> EditUserRoles(UserViewModel model) { ... }
            // [HttpPost]
            // public async Task<IActionResult> DeleteUser(string id) { ... } // To delete a user
        }
    }
    