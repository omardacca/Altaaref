using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefAPI
{
    public class AltaarefContext : DbContext
    {
        public AltaarefContext(DbContextOptions<AltaarefContext> options)
            :base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<CourseFaculty> CourseFaculty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many to Many - Faculty, Course and CourseFaculty
            modelBuilder.Entity<CourseFaculty>()
                .HasKey(cf => new { cf.CourseId, cf.FacultyId });

            modelBuilder.Entity<CourseFaculty>()
                .HasOne(cf => cf.Course)
                .WithMany(c => c.CourseFaculty)
                .HasForeignKey(cf => cf.CourseId);

            modelBuilder.Entity<CourseFaculty>()
                .HasOne(cf => cf.Faculty)
                .WithMany(f => f.CourseFaculty)
                .HasForeignKey(cf => cf.FacultyId);
        }
    }
}
