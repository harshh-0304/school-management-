// schoolmanagement/Models/Teacher.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // For ICollection

namespace schoolmanagement.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string SubjectExpertise { get; set; } // e.g., "Mathematics", "English"

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        // Navigation property for students (One-to-Many: Teacher -> Students)
        public ICollection<Student> Students { get; set; } = new List<Student>();

        // Navigation property for TeacherSubjects (Many-to-Many: Teacher -> TeacherSubject)
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    }
}