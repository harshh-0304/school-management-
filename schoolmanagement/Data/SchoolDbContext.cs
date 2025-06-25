// schoolmanagement/Data/SchoolDbContext.cs
using Microsoft.EntityFrameworkCore;
using schoolmanagement.Models; // CORRECT: Points to your Student class

namespace schoolmanagement.Data // CORRECT: Namespace for your DbContext
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } 
    }
}