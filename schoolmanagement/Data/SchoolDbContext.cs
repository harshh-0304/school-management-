// schoolmanagement/Data/SchoolDbContext.cs

using Microsoft.EntityFrameworkCore;
using schoolmanagement.Models; // Ensure this is here

namespace schoolmanagement.Data
{
    public class SchoolDbContext : DbContext // <--- Inherits from DbContext ONLY
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<Class> Classes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Always call the base implementation first



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




            modelBuilder.Entity<Teacher>()


                .HasMany(t => t.Students)

                .WithOne(s => s.Teacher)

                .HasForeignKey(s => s.TeacherId);

        }
    }
}