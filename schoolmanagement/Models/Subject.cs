// schoolmanagement/Models/Subject.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // For ICollection

namespace schoolmanagement.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        // Navigation property for TeacherSubjects (Many-to-Many: Subject -> TeacherSubject)
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    }
}