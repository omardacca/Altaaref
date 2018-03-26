﻿using AltaarefWebAPI.Models;
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
        public DbSet<Notebook> Notebook { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentFavNotebooks> StudentFavNotebooks { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Many to Many - FacultyCourses, Faculty, Courses
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

            // One to Many - Courses, Notebook

            modelBuilder.Entity<Notebook>()
                .HasOne(n => n.Course)
                .WithMany(c => c.Notebooks)
                .HasForeignKey(n => n.CourseId);

            // Set default value for columns in Notebook Table
            
            modelBuilder.Entity<Notebook>()
                .Property(n => n.ViewsCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<Notebook>()
                .Property(n => n.PublishDate)
                .HasDefaultValueSql("getdate()");

            // Many to Many - StudentFavNotebooks, Student, Notebook

            modelBuilder.Entity<StudentFavNotebooks>()
                .HasKey(sfn => new { sfn.NotebookId, sfn.StudentId });

            modelBuilder.Entity<StudentFavNotebooks>()
                .HasOne(sfn => sfn.Student)
                .WithMany(s => s.StudentFavNotebooks)
                .HasForeignKey(sfn => sfn.StudentId);

            modelBuilder.Entity<StudentFavNotebooks>()
                .HasOne(sfn => sfn.Notebook)
                .WithMany(s => s.StudentFavNotebooks)
                .HasForeignKey(sfn => sfn.NotebookId);

            // Many to Many - StudyGroup, Student, Course

            modelBuilder.Entity<StudyGroup>()
                .HasKey(sg => new { sg.CourseId, sg.StudentId });

            modelBuilder.Entity<StudyGroup>()
                .HasOne(sg => sg.Student)
                .WithMany(s => s.StudyGroups)
                .HasForeignKey(sg => sg.StudentId);

            modelBuilder.Entity<StudyGroup>()
                .HasOne(sg => sg.Course)
                .WithMany(s => s.StudyGroups)
                .HasForeignKey(fc => fc.CourseId);

            // Many to Many - Student Faculty

            modelBuilder.Entity<StudentFaculty>()
                .HasKey(sf => new { sf.FacultyId, sf.StudentId });

            modelBuilder.Entity<StudentFaculty>()
                .HasOne(sg => sg.Student)
                .WithMany(s => s.StudentFaculty)
                .HasForeignKey(sg => sg.StudentId);

            modelBuilder.Entity<StudentFaculty>()
                .HasOne(sf => sf.Faculty)
                .WithMany(s => s.StudentFaculty)
                .HasForeignKey(fc => fc.FacultyId);

        }
    }
}
