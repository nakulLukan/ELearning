using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChapterTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LookupValues_ChapterNameLookupId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ChapterNameLookupId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ChapterNameLookupId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Lessons",
                newName: "ChapterId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                newName: "IX_Lessons_ChapterId");

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SubjectId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_SubjectId",
                table: "Chapters",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Chapters_ChapterId",
                table: "Lessons",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Chapters_ChapterId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.RenameColumn(
                name: "ChapterId",
                table: "Lessons",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ChapterId",
                table: "Lessons",
                newName: "IX_Lessons_SubjectId");

            migrationBuilder.AddColumn<int>(
                name: "ChapterNameLookupId",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ChapterNameLookupId",
                table: "Lessons",
                column: "ChapterNameLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LookupValues_ChapterNameLookupId",
                table: "Lessons",
                column: "ChapterNameLookupId",
                principalTable: "LookupValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }
    }
}
