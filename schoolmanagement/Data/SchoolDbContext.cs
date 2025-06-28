// schoolmanagement/Data/SchoolDbContext.cs

using Microsoft.EntityFrameworkCore;
using schoolmanagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // <--- ADDED/ENSURED
using schoolmanagement.Models.Auth; // <--- ADDED/ENSURED for ApplicationUser location

namespace schoolmanagement.Data
{
    // *** CRITICAL CHANGE HERE: Inherit from IdentityDbContext<ApplicationUser> ***
    public class SchoolDbContext : IdentityDbContext<ApplicationUser> // <--- MODIFIED
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
      
        public DbSet<Class> Classes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // *** IMPORTANT: Call the base implementation for Identity first ***
            base.OnModelCreating(modelBuilder); // Always call the base for IdentityDbContext

            // Configure the Many-to-Many Relationship between Teacher and Subject


            modelBuilder.Entity<TeacherSubject>()

                .HasKey(ts => new { ts.TeacherId, ts.SubjectId });


            modelBuilder.Entity<TeacherSubject>()

                .HasOne(ts => ts.Teacher)

                .WithMany(t => t.TeacherSubjects)

                .HasForeignKey(ts => ts.TeacherId);



            modelBuilder.Entity<TeacherSubject>()

                .HasOne(ts => ts.Subject)

                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId);

            // Configure the One-to-Many Relationship between Teacher and Student

            modelBuilder.Entity<Teacher>()


                .HasMany(t => t.Students)

                .WithOne(s => s.Teacher)

                .HasForeignKey(s => s.TeacherId);

        }
    }
}