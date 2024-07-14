using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class QuizTablesAdded3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestionAnswers_Attachments_AnswerImageId1",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestionAnswers_AnswerImageId1",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "AnswerImageId1",
                table: "QuizQuestionAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "QuizQuestionAnswers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AnswerImageId",
                table: "QuizQuestionAnswers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionAnswers_AnswerImageId",
                table: "QuizQuestionAnswers",
                column: "AnswerImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestionAnswers_Attachments_AnswerImageId",
                table: "QuizQuestionAnswers",
                column: "AnswerImageId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestionAnswers_Attachments_AnswerImageId",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestionAnswers_AnswerImageId",
                table: "QuizQuestionAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "QuizQuestionAnswers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AnswerImageId",
                table: "QuizQuestionAnswers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AnswerImageId1",
                table: "QuizQuestionAnswers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionAnswers_AnswerImageId1",
                table: "QuizQuestionAnswers",
                column: "AnswerImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestionAnswers_Attachments_AnswerImageId1",
                table: "QuizQuestionAnswers",
                column: "AnswerImageId1",
                principalTable: "Attachments",
                principalColumn: "Id");
        }
    }
}
