using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FullNameColAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserOtherDetails_UserId",
                table: "ApplicationUserOtherDetails");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "ApplicationUserOtherDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserOtherDetails_UserId",
                table: "ApplicationUserOtherDetails",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserOtherDetails_UserId",
                table: "ApplicationUserOtherDetails");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "ApplicationUserOtherDetails");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserOtherDetails_UserId",
                table: "ApplicationUserOtherDetails",
                column: "UserId");
        }
    }
}
