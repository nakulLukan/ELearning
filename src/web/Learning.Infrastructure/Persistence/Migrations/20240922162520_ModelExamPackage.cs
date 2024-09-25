using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModelExamPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "ModelExamConfigurations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ModelExamConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "ModelExamPackageId",
                table: "ModelExamConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ModelExamPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<float>(type: "real", nullable: false),
                    DiscountedPrice = table.Column<float>(type: "real", nullable: false),
                    ExamNotificationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamPackages_ExamNotifications_ExamNotificationId",
                        column: x => x.ExamNotificationId,
                        principalTable: "ExamNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamConfigurations_ModelExamPackageId",
                table: "ModelExamConfigurations",
                column: "ModelExamPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamPackages_ExamNotificationId",
                table: "ModelExamPackages",
                column: "ExamNotificationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfigurations_ModelExamPackages_ModelExamPackageId",
                table: "ModelExamConfigurations",
                column: "ModelExamPackageId",
                principalTable: "ModelExamPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfigurations_ModelExamPackages_ModelExamPackageId",
                table: "ModelExamConfigurations");

            migrationBuilder.DropTable(
                name: "ModelExamPackages");

            migrationBuilder.DropIndex(
                name: "IX_ModelExamConfigurations_ModelExamPackageId",
                table: "ModelExamConfigurations");

            migrationBuilder.DropColumn(
                name: "ModelExamPackageId",
                table: "ModelExamConfigurations");

            migrationBuilder.AddColumn<float>(
                name: "DiscountedPrice",
                table: "ModelExamConfigurations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "ModelExamConfigurations",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
