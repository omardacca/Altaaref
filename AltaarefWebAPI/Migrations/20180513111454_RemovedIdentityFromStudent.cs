using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class RemovedIdentityFromStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_IdentityId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_IdentityId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IdenitytyId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Student");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdenitytyId",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                table: "Student",
                nullable: true);

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
