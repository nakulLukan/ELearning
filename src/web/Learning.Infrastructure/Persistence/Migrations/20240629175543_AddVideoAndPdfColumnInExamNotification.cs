using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoAndPdfColumnInExamNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GovtLink",
                table: "ExamNotifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PdfFileId",
                table: "ExamNotifications",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VideoId",
                table: "ExamNotifications",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RelativePath = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    LastUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamNotifications_PdfFileId",
                table: "ExamNotifications",
                column: "PdfFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamNotifications_VideoId",
                table: "ExamNotifications",
                column: "VideoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamNotifications_Attachments_PdfFileId",
                table: "ExamNotifications",
                column: "PdfFileId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamNotifications_Attachments_VideoId",
                table: "ExamNotifications",
                column: "VideoId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamNotifications_Attachments_PdfFileId",
                table: "ExamNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamNotifications_Attachments_VideoId",
                table: "ExamNotifications");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_ExamNotifications_PdfFileId",
                table: "ExamNotifications");

            migrationBuilder.DropIndex(
                name: "IX_ExamNotifications_VideoId",
                table: "ExamNotifications");

            migrationBuilder.DropColumn(
                name: "GovtLink",
                table: "ExamNotifications");

            migrationBuilder.DropColumn(
                name: "PdfFileId",
                table: "ExamNotifications");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "ExamNotifications");
        }
    }
}
