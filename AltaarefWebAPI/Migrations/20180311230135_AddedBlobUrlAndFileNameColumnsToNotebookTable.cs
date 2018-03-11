using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddedBlobUrlAndFileNameColumnsToNotebookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobURL",
                table: "Notebook",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Notebook",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobURL",
                table: "Notebook");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Notebook");
        }
    }
}
