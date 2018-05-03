using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class OneToManyStudentNotebook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Notebook",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notebook_StudentId",
                table: "Notebook",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notebook_Student_StudentId",
                table: "Notebook",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notebook_Student_StudentId",
                table: "Notebook");

            migrationBuilder.DropIndex(
                name: "IX_Notebook_StudentId",
                table: "Notebook");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Notebook");
        }
    }
}
