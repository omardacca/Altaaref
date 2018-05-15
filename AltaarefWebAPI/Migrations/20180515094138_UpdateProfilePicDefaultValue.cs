using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class UpdateProfilePicDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicBlobUrl",
                table: "Student",
                nullable: false,
                defaultValue: "https://csb08eb270fff55x4a98xb1a.blob.core.windows.net/notebooks/defaultprofpic.png",
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicBlobUrl",
                table: "Student",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValue: "https://csb08eb270fff55x4a98xb1a.blob.core.windows.net/notebooks/defaultprofpic.png");
        }
    }
}
