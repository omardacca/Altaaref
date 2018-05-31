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
    [Migration("20180531012558_AddedRideTable")]
    partial class AddedRideTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AltaarefWebAPI.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfilePicBlobUrl");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("StudentId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

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

            modelBuilder.Entity("AltaarefWebAPI.Models.Ride", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("FromAddress");

                    b.Property<string>("FromCity");

                    b.Property<double>("FromLat");

                    b.Property<double>("FromLong");

                    b.Property<string>("Message");

                    b.Property<byte>("NumOfFreeSeats");

                    b.Property<DateTime>("Time");

                    b.Property<string>("ToAddress");

                    b.Property<string>("ToCity");

                    b.Property<double>("ToLat");

                    b.Property<double>("ToLong");

                    b.HasKey("Id");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("AltaarefWebAPI.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DOB");

                    b.Property<string>("FullName");

                    b.Property<string>("IdentityId");

                    b.Property<string>("IdentityId1");

                    b.Property<string>("ProfilePicBlobUrl")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("https://csb08eb270fff55x4a98xb1a.blob.core.windows.net/notebooks/defaultprofpic.png");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId1");

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

            modelBuilder.Entity("AltaarefWebAPI.Models.UserNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<int>("StudentId");

                    b.Property<string>("Title");

                    b.Property<string>("Topic");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("UserNotifications");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
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

            modelBuilder.Entity("AltaarefWebAPI.Models.Student", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.AppUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId1");
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

            modelBuilder.Entity("AltaarefWebAPI.Models.UserNotification", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.Student", "Student")
                        .WithMany("UserNotifications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AltaarefWebAPI.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AltaarefWebAPI.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
