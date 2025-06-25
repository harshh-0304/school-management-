// schoolmanagement/Models/Student.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace schoolmanagement.Models 
{
    public class Student
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}