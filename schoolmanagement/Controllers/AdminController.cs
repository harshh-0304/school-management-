using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; // Required for UserManager/RoleManager
using schoolmanagement.Models.Auth; // Required for ApplicationUser
using schoolmanagement.ViewModels; // Crucial for UserViewModel, EditUserViewModel
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
                    Id = user.Id!, // Use null-forgiving operator as Id is typically non-null
                    Email = user.Email!, // Use null-forgiving operator for Email
                    Roles = roles.ToList() // List of roles (e.g., "Admin", "Teacher")
                });
            }

            // Pass the populated list of usersWithRoles to the ManageUsers view
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

            // Get all roles from the database
            var allRoles = await _roleManager.Roles.ToListAsync();
            // Get roles currently assigned to this user
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
        [ValidateAntiForgeryToken] // Protects against Cross-Site Request Forgery (CSRF) attacks
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

                // Update user email (if changed)
                if (user.Email != model.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        foreach (var error in setEmailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        // If email update fails, redisplay form with errors
                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }
                    // UserName is typically derived from Email for Identity, so update it too
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


                // Update user roles
                var userRoles = await _userManager.GetRolesAsync(user);
                var selectedRoles = model.SelectedRoles ?? new List<string>(); // Handle null SelectedRoles

                // Remove roles not selected
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
                        // If role update fails, redisplay form with errors
                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }
                }

                // Add roles newly selected
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
                        // If role update fails, redisplay form with errors
                        model.AllRoles = (await _roleManager.Roles.ToListAsync())
                            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = r.Name, Value = r.Name })
                            .ToList();
                        return View(model);
                    }
                }

                TempData["SuccessMessage"] = $"User {user.Email} updated successfully!";
                return RedirectToAction(nameof(ManageUsers));
            }

            // If ModelState is not valid, re-populate AllRoles and return the view
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
                // If deletion fails, return to the confirmation page with errors
                return View("DeleteUser", user); // Pass the user back to the delete view
            }
        }
    }
}
