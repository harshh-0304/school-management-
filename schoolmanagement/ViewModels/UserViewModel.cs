
    // schoolmanagement/ViewModels/UserViewModel.cs (Existing content + new class)
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; // Required for [Required]

    namespace schoolmanagement.ViewModels
    {
        // Existing UserViewModel class (for listing users)
        public class UserViewModel
        {
            public string Id { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = new List<string>();
        }

        // Existing ViewModel for editing user details and roles
        public class EditUserViewModel
        {
            public string Id { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;


        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> AllRoles { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();


        public List<string> SelectedRoles { get; set; } = new List<string>();
        }

        // NEW: ViewModel for Admin Dashboard summary data
        public class AdminDashboardViewModel
        {
            public int TotalStudents { get; set; }
            public int TotalTeachers { get; set; }
            public int TotalClasses { get; set; } // Added for completeness, if you want to display this later
            public int TotalSubjects { get; set; } // Added for completeness, if you want to display this later
        }
    }
    