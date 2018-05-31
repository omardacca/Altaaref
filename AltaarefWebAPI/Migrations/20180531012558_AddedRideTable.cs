using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddedRideTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    FromAddress = table.Column<string>(nullable: true),
                    FromCity = table.Column<string>(nullable: false),
                    FromLat = table.Column<double>(nullable: false),
                    FromLong = table.Column<double>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    NumOfFreeSeats = table.Column<byte>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    ToAddress = table.Column<string>(nullable: true),
                    ToCity = table.Column<string>(nullable: false),
                    ToLat = table.Column<double>(nullable: false),
                    ToLong = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rides");
        }
    }
}
