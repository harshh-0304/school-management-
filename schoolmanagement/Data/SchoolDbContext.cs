// schoolmanagement/Data/SchoolDbContext.cs

// Required for DbContext functionality
using Microsoft.EntityFrameworkCore;

// Required to reference your model classes (Student, Teacher, Subject, TeacherSubject)
using schoolmanagement.Models;

namespace schoolmanagement.Data // This namespace MUST match your project's 'Data' folder structure
{
    public class SchoolDbContext : DbContext
    {
        // Constructor: This is how your DbContext receives its configuration (like the connection string)
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        // --- DbSet Properties: These represent your tables in the database ---
        public DbSet<Student> Students { get; set; } 
        public DbSet<Teacher> Teachers { get; set; } // New: DbSet for the Teachers table
        public DbSet<Subject> Subjects { get; set; } // New: DbSet for the Subjects table
        public DbSet<TeacherSubject> TeacherSubjects { get; set; } // New: DbSet for the Teacher-Subject linking table
        public DbSet<Class> Classes { get; set; }

        // --- OnModelCreating Method: This is where you define complex relationships not handled by convention ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Always call the base implementation first

            // --- Configure the Many-to-Many Relationship between Teacher and Subject ---
            // This relationship is managed through the 'TeacherSubject' join entity.
            modelBuilder.Entity<TeacherSubject>()
                // Define the composite primary key for the TeacherSubject linking table.
                // This means the combination of TeacherId and SubjectId uniquely identifies a row.
                .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

            modelBuilder.Entity<TeacherSubject>()
                // A TeacherSubject entry has one Teacher.
                .HasOne(ts => ts.Teacher)
                // The Teacher entity can have many TeacherSubject entries.
                .WithMany(t => t.TeacherSubjects)
                // The foreign key in the TeacherSubject table pointing to Teacher is TeacherId.
                .HasForeignKey(ts => ts.TeacherId);

            modelBuilder.Entity<TeacherSubject>()
                // A TeacherSubject entry has one Subject.
                .HasOne(ts => ts.Subject)
                // The Subject entity can have many TeacherSubject entries.
                .WithMany(s => s.TeacherSubjects)
                // The foreign key in the TeacherSubject table pointing to Subject is SubjectId.
                .HasForeignKey(ts => ts.SubjectId);

            // --- Configure the One-to-Many Relationship between Teacher and Student ---
            // A Teacher can be a homeroom teacher for many Students.
            modelBuilder.Entity<Teacher>()
                // A Teacher entity has many Students.
                .HasMany(t => t.Students)
                // Each Student entity has one Teacher.
                .WithOne(s => s.Teacher)
                // The foreign key in the Student table that links to Teacher is TeacherId.
                .HasForeignKey(s => s.TeacherId);
            // By default, EF Core might configure cascade delete here.
            // If you want to prevent students from being deleted if their teacher is deleted,
            // you would add: .OnDelete(DeleteBehavior.Restrict);
            // But for now, the default behavior (often Cascade) is fine for initial setup.
        }
    }
}