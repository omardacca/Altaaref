using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class ResetAllUsersIssues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Student");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityId1",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_IdentityId1",
                table: "Student",
                column: "IdentityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_IdentityId1",
                table: "Student",
                column: "IdentityId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
