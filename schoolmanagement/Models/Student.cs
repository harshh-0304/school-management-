using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Ensure this is present at the top

namespace schoolmanagement.Models
{
    public class Student
    {
        public int Id { get; set; } // Your primary key might be named differently (e.g., StudentId)

        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; } // Or Nullable DateTime? DateOfBirth { get; set; } if appropriate

        [StringLength(10)]
        public string Gender { get; set; } // Or string? Gender { get; set; } if appropriate

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } // Or string? PhoneNumber { get; set; } if appropriate

        // Foreign key for Teacher
        [Display(Name = "Homeroom Teacher")]
        public int? TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; } // <--- CRITICAL FIX: MADE NULLABLE

        // Foreign key for Class
        [Display(Name = "Class")]
        public int? ClassId { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; } // <--- CRITICAL FIX: MADE NULLABLE
    }
}