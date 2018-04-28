using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AltaarefWebAPI.Migrations
{
    public partial class AddedStudyGroupCommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudyGroupComment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: false),
                    FullTime = table.Column<DateTime>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    StudyGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGroupComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyGroupComment_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyGroupComment_StudyGroups_StudyGroupId",
                        column: x => x.StudyGroupId,
                        principalTable: "StudyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroupComment_StudentId",
                table: "StudyGroupComment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroupComment_StudyGroupId",
                table: "StudyGroupComment",
                column: "StudyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyGroupComment");
        }
    }
}
