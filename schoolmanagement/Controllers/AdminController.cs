using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using schoolmanagement.Data; // <--- ADDED for SchoolDbContext
using schoolmanagement.Models.Auth;
using schoolmanagement.ViewModels; // Crucial for UserViewModel, EditUserViewModel, AdminDashboardViewModel
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace schoolmanagement.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolDbContext _context; // <--- ADDED for database access

        // Constructor to inject UserManager, RoleManager, and SchoolDbContext
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SchoolDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context; // <--- Assigned here
        }

        // GET: /Admin/Index
        // Main entry point for the Admin Dashboard. Now fetches counts.
        public async Task<IActionResult> Index() // Make it async
        {
            ViewData["Title"] = "Admin Dashboard";

            // Fetch counts from the database
            var totalStudents = await _context.Students.CountAsync();
            var totalTeachers = await _context.Teachers.CountAsync();
            var totalClasses = await _context.Classes.CountAsync(); // Fetching for completeness
            var totalSubjects = await _context.Subjects.CountAsync(); // Fetching for completeness

            var model = new AdminDashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalClasses = totalClasses,
                TotalSubjects = totalSubjects
            };

            return View(model); // Pass the model to the view
        }

        // GET: /Admin/ManageUsers
        // This action will retrieve and display a list of all users and their roles.
        public async Task<IActionResult> ManageUsers()
        {
            ViewData["Title"] = "Manage Users";



            var users = await _userManager.Users.ToListAsync();


            var usersWithRoles = new List<UserViewModel>();


            foreach (var user in users)

            {

                var roles = await _userManager.GetRolesAsync(user);


                usersWithRoles.Add(new UserViewModel

                {
                    Id = user.Id!,
                    Email = user.Email!,
                    Roles = roles.ToList()
                });
            }


            return View(usersWithRoles);
        }

        // GET: /Admin/EditUser/{id}
        // Displays the form to edit a user's details and roles.
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }


            var allRoles = await _roleManager.Roles.ToListAsync();

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                AllRoles = allRoles.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userRoles.Contains(r.Name!)
                }).ToList()
            };

            ViewData["Title"] = $"Edit User: {user.Email}";
            return View(model);
        }

        // POST: /Admin/EditUser/{id}
        // Processes the form submission for editing a user.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }


                if (user.Email != model.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        foreach (var error in setEmailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }

                    var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                    if (!setUserNameResult.Succeeded)
                    {
                        foreach (var error in setUserNameResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }
                }



                var userRoles = await _userManager.GetRolesAsync(user);
                var selectedRoles = model.SelectedRoles ?? new List<string>();





                var rolesToRemove = userRoles.Except(selectedRoles);



                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        foreach (var error in removeResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);

                        }

                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }
                }


                var rolesToAdd = selectedRoles.Except(userRoles);
                if (rolesToAdd.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        foreach (var error in addResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }
                }

                TempData["SuccessMessage"] = $"User {user.Email} updated successfully!";
                return RedirectToAction(nameof(ManageUsers));
            }


            model.AllRoles = (await _roleManager.Roles.ToListAsync())
                .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                .ToList();
            return View(model);
        }

        // GET: /Admin/DeleteUser/{id}
        // Displays a confirmation page before deleting a user.
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Title"] = $"Delete User: {user.Email}";
            return View(user);
        }

        // POST: /Admin/DeleteUser/{id}
        // Processes the deletion of a user.
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User {user.Email} deleted successfully!";
                return RedirectToAction(nameof(ManageUsers));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("DeleteUser", user);
            }
        }
    }
}
