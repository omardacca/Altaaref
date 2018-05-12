﻿// <auto-generated />
using AltaarefWebAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AltaarefWebAPI.Migrations
{
    [DbContext(typeof(AltaarefDbContext))]
    partial class AltaarefDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AltaarefWebAPI.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.FacultyCourse", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<int>("FacultyId");

                    b.HasKey("CourseId", "FacultyId");

                    b.HasIndex("FacultyId");

                    b.ToTable("FacultyCourse");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.HelpFaculty", b =>
                {
                    b.Property<int>("HelpRequestId");

                    b.Property<int>("FacultyId");

                    b.HasKey("HelpRequestId", "FacultyId");

                    b.HasIndex("FacultyId");

                    b.ToTable("HelpFaculty");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.HelpRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsGeneral")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsMet")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Message");

                    b.Property<int>("StudentId");

                    b.Property<int>("Views")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("HelpRequest");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.HelpRequestComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<int>("HelpRequestId");

                    b.HasKey("Id");

                    b.HasIndex("HelpRequestId");

                    b.ToTable("HelpRequestComment");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.Notebook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BlobURL");

                    b.Property<int>("CourseId");

                    b.Property<string>("Name");

                    b.Property<DateTime>("PublishDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("StudentId");

                    b.Property<int>("ViewsCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Notebook");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.NotebookRates", b =>
                {
                    b.Property<int>("NotebookId");

                    b.Property<int>("StudentId");

                    b.Property<byte>("Rate");

                    b.HasKey("NotebookId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("NotebookRates");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DOB");

                    b.Property<string>("FullName");

                    b.Property<string>("ProfilePicBlobUrl")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudentCourses", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<int>("StudentId");

                    b.HasKey("CourseId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudentFaculty", b =>
                {
                    b.Property<int>("FacultyId");

                    b.Property<int>("StudentId");

                    b.HasKey("FacultyId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentFaculties");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudentFavNotebooks", b =>
                {
                    b.Property<int>("NotebookId");

                    b.Property<int>("StudentId");

                    b.HasKey("NotebookId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentFavNotebooks");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Message");

                    b.Property<int>("StudentId");

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudyGroups");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroupAttendants", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("StudentId");

                    b.Property<int>("StudyGroupId");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("StudyGroupId");

                    b.ToTable("StudyGroupAttendants");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroupComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("FullTime");

                    b.Property<int>("StudentId");

                    b.Property<int>("StudyGroupId");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("StudyGroupId");

                    b.ToTable("StudyGroupComment");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroupInvitations", b =>
                {
                    b.Property<int>("StudentId");

                    b.Property<int>("StudyGroupId");

                    b.Property<bool>("VerificationStatus");

                    b.HasKey("StudentId", "StudyGroupId");

                    b.HasIndex("StudyGroupId");

                    b.ToTable("StudyGroupInvitations");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.FacultyCourse", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Course", "Course")
                        .WithMany("FacultyCourse")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Faculty", "Faculty")
                        .WithMany("FacultyCourse")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.HelpFaculty", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Faculty", "Faculty")
                        .WithMany("HelpFaculties")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.HelpRequest", "HelpRequest")
                        .WithMany("HelpFaculties")
                        .HasForeignKey("HelpRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.HelpRequest", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("HelpRequests")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.HelpRequestComment", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.HelpRequest", "HelpRequest")
                        .WithMany("Comments")
                        .HasForeignKey("HelpRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.Notebook", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Course", "Course")
                        .WithMany("Notebooks")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("Notebooks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.NotebookRates", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Notebook", "Notebook")
                        .WithMany("NotebookRates")
                        .HasForeignKey("NotebookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("NotebookRates")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudentCourses", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudentCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudentFaculty", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Faculty", "Faculty")
                        .WithMany("StudentFaculty")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudentFaculty")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudentFavNotebooks", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Notebook", "Notebook")
                        .WithMany("StudentFavNotebooks")
                        .HasForeignKey("NotebookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudentFavNotebooks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroup", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Course", "Course")
                        .WithMany("StudyGroups")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudyGroups")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroupAttendants", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudyGroupAttendants")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.StudyGroup", "StudyGroup")
                        .WithMany("StudyGroupAttendants")
                        .HasForeignKey("StudyGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroupComment", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudyGroupComments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.StudyGroup", "StudyGroup")
                        .WithMany("StudyGroupComments")
                        .HasForeignKey("StudyGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.StudyGroupInvitations", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("StudyGroupInvitations")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.StudyGroup", "StudyGroup")
                        .WithMany("StudyGroupInvitations")
                        .HasForeignKey("StudyGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
