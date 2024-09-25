using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveColAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "ModelExamQuestionConfigurations");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ModelExamQuestionConfigurations",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ModelExamQuestionConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ModelExamQuestionConfigurations");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "ModelExamQuestionConfigurations");

            migrationBuilder.AddColumn<bool>(
                name: "Mark",
                table: "ModelExamQuestionConfigurations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
