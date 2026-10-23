using Microsoft.EntityFrameworkCore;
using StdManagment.Models;

namespace StdManagment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Student>().HasIndex(s => s.Email).IsUnique();
            b.Entity<Student>().HasIndex(s => s.StudentId).IsUnique();

            // Database level par sirf EK Admin allowed
            b.Entity<Student>().HasIndex(s => s.Role)
                .IsUnique()
                .HasFilter("[Role] = 'Admin'");
        }
    }
}