// schoolmanagement/Models/TeacherSubject.cs
namespace schoolmanagement.Models
{
    public class TeacherSubject
    {
        // Composite Primary Key (Fluent API will define this)
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        // Navigation properties to the related entities
        public Teacher Teacher { get; set; } = default!; // Default! tells the compiler it will be initialized
        public Subject Subject { get; set; } = default!;
    }
}