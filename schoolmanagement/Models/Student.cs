// schoolmanagement/Models/Student.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // For [ForeignKey]

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

        // Foreign Key for Teacher (One-to-Many: Student -> Teacher)
        [Display(Name = "Homeroom Teacher")] // Display name for UI
        public int TeacherId { get; set; } 

        // Navigation property to the Teacher
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; } = default!; // Default! tells the compiler it will be initialized
    }
}