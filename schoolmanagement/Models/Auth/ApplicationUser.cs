// schoolmanagement/Models/Auth/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations; // Required for [Required]

namespace schoolmanagement.Models.Auth
{
    // Extend IdentityUser to add custom properties for your application's users
    public class ApplicationUser : IdentityUser
    {
        // Add a Name property for the user's display name
        [Required] // Make Name a required field
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public required string Name { get; set; } // Using 'required' for C# 11+
    }
}
