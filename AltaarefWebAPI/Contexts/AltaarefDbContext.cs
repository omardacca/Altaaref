using AltaarefWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Contexts
{
    public class AltaarefDbContext : DbContext
    {
        public AltaarefDbContext(DbContextOptions<AltaarefDbContext> options) : base(options) { }
        public AltaarefDbContext() { }
        
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<FacultyCourse> FacultyCourse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacultyCourse>()
                .HasKey(fc => new { fc.CourseId, fc.FacultyId });

            modelBuilder.Entity<FacultyCourse>()
                .HasOne(fc => fc.Faculty)
                .WithMany(f => f.FacultyCourse)
                .HasForeignKey(fc => fc.FacultyId);

            modelBuilder.Entity<FacultyCourse>()
                .HasOne(fc => fc.Course)
                .WithMany(c => c.FacultyCourse)
                .HasForeignKey(fc => fc.CourseId);


        }
    }
}
