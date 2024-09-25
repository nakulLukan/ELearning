using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModelExamPackage1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPackages_ExamNotifications_ExamNotificationId",
                table: "ModelExamPackages");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPackages_ExamNotifications_ExamNotificationId",
                table: "ModelExamPackages",
                column: "ExamNotificationId",
                principalTable: "ExamNotifications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPackages_ExamNotifications_ExamNotificationId",
                table: "ModelExamPackages");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPackages_ExamNotifications_ExamNotificationId",
                table: "ModelExamPackages",
                column: "ExamNotificationId",
                principalTable: "ExamNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
