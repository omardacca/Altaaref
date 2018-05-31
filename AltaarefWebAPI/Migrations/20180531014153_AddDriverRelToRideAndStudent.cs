using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddDriverRelToRideAndStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                table: "Rides",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Student_DriverId",
                table: "Rides",
                column: "DriverId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Student_DriverId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DriverId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Rides");
        }
    }
}
