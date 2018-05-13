using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class TryToInitUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_IdentityId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_IdentityId",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "AspNetUsers",
                newName: "ProfilePicBlobUrl");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityId",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityId1",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_IdentityId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_IdentityId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IdentityId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePicBlobUrl",
                table: "AspNetUsers",
                newName: "EmailAddress");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityId",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_IdentityId",
                table: "Student",
                column: "IdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_IdentityId",
                table: "Student",
                column: "IdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
