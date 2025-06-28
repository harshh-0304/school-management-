// schoolmanagement/Models/Class.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace schoolmanagement.Models
{
    public class Class
    {
        // Renamed 'Id' to 'ClassId' to match the errors in your controllers/views.
        // This is assumed based on the errors, as the code expects 'ClassId'.
        [Key] // Explicitly mark as primary key, though 'Id' or 'ClassNameId' is usually inferred
        public int ClassId { get; set; }

        [Required]
        public required string Name { get; set; }

        // Added 'Description' property as it was missing and causing errors
        [Required]
        public required string Description { get; set; } // Added this property

        // Navigation property for students in this class
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
