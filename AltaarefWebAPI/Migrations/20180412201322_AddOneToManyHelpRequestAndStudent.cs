using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddOneToManyHelpRequestAndStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "HelpRequest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HelpRequest_StudentId",
                table: "HelpRequest",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_HelpRequest_Student_StudentId",
                table: "HelpRequest",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HelpRequest_Student_StudentId",
                table: "HelpRequest");

            migrationBuilder.DropIndex(
                name: "IX_HelpRequest_StudentId",
                table: "HelpRequest");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "HelpRequest");
        }
    }
}
