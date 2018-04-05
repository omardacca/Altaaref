using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddRelationWithStudentAndCoursesStudyGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "StudyGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudyGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_Course_CourseId",
                table: "StudyGroups",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_Student_StudentId",
                table: "StudyGroups",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_Course_CourseId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_Student_StudentId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_CourseId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_StudentId",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudyGroups");
        }
    }
}
