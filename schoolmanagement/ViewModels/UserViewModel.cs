    // schoolmanagement/ViewModels/UserViewModel.cs (Existing content + new class)
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; // Required for [Required]

    namespace schoolmanagement.ViewModels
    {
        // Existing UserViewModel class (for listing users)
        public class UserViewModel
        {
            public string Id { get; set; } = string.Empty; // Initialized for CS8618
            public string Email { get; set; } = string.Empty; // Initialized for CS8618
            public List<string> Roles { get; set; } = new List<string>();
        }

        // NEW: ViewModel for editing user details and roles
        public class EditUserViewModel
        {
            public string Id { get; set; } = string.Empty; // User Id, hidden field

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty; // User's email

            // Used to display all available roles in a multi-select/checkbox list
            // Populated in the GET action of EditUser
            public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> AllRoles { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

            // Used to bind the selected roles from the form
            // Each string here will be a role name (e.g., "Admin", "Teacher")
            public List<string> SelectedRoles { get; set; } = new List<string>();
        }
    }
    