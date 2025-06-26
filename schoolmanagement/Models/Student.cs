// schoolmanagement/Models/Student.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace schoolmanagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        // --- CHANGE THIS LINE: Make TeacherId nullable by adding '?' ---
        [Display(Name = "Homeroom Teacher")]
        public int? TeacherId { get; set; } // Changed from 'int' to 'int?'

        // Navigation property to the Teacher
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; } // Changed from 'Teacher' to 'Teacher?'
    }
}