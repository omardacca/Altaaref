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
    [Migration("20180310192229_AddDefValToPublishDateOfNotebookTable")]
    partial class AddDefValToPublishDateOfNotebookTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("AltaarefWebAPI.Models.Notebook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("Name");

                    b.Property<DateTime>("PublishDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("ViewsCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Notebook");
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

            modelBuilder.Entity("AltaarefWebAPI.Models.Notebook", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Course", "Course")
                        .WithMany("Notebooks")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
