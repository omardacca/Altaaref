using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddRideCommentsTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "IsPublic",
            //    table: "Notebook",
            //    newName: "IsPrivate");

            migrationBuilder.CreateTable(
                name: "RideComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: false),
                    FullTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2018, 6, 6, 20, 36, 57, 580, DateTimeKind.Local)),
                    RideId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideComments_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideComments_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideComments_RideId",
                table: "RideComments",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideComments_StudentId",
                table: "RideComments",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideComments");

            //migrationBuilder.RenameColumn(
            //    name: "IsPrivate",
            //    table: "Notebook",
            //    newName: "IsPublic");
        }
    }
}
