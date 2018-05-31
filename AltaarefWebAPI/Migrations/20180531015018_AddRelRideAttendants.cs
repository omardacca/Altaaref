using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddRelRideAttendants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideAttendants",
                columns: table => new
                {
                    RideId = table.Column<int>(nullable: false),
                    AttendantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideAttendants", x => new { x.RideId, x.AttendantId });
                    table.ForeignKey(
                        name: "FK_RideAttendants_Student_AttendantId",
                        column: x => x.AttendantId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideAttendants_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideAttendants_AttendantId",
                table: "RideAttendants",
                column: "AttendantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideAttendants");
        }
    }
}
