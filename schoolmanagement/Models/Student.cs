// schoolmanagement/Models/Student.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System; // Required for DateTime

namespace schoolmanagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; } // Added 'required'

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public required string Gender { get; set; } // Added 'required'

        [Required]
        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; } // Added 'required'

        // Foreign key for Class
        [Display(Name = "Class")]
        public int ClassId { get; set; }
        public Class? Class { get; set; } // Made nullable as navigation properties can be null

        // Foreign key for Teacher
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; } // Made nullable as navigation properties can be null
    }
}
