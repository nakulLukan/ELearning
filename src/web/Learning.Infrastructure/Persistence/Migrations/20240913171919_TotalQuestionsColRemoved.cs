using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TotalQuestionsColRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfigurations_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfigurations");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ModelExamConfigurations");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "ModelExamConfigurations");

            migrationBuilder.AlterColumn<long>(
                name: "ExamSolutionVideoId",
                table: "ModelExamConfigurations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfigurations_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfigurations",
                column: "ExamSolutionVideoId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfigurations_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfigurations");

            migrationBuilder.AlterColumn<long>(
                name: "ExamSolutionVideoId",
                table: "ModelExamConfigurations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ModelExamConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "ModelExamConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfigurations_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfigurations",
                column: "ExamSolutionVideoId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
