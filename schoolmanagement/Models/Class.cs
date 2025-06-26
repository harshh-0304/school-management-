using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace schoolmanagement.Models
{
    public class Class
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Class name is required.")]
        [StringLength(50, ErrorMessage = "Class name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; } // <<-- CHANGED: Made nullable with '?'

        // Navigation property for students in this class
        public ICollection<Student>? Students { get; set; } = new List<Student>(); // <<-- CHANGED: Made nullable with '?' AND initialized
    }
}