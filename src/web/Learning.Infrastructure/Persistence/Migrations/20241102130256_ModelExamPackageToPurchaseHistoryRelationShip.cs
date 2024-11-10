using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModelExamPackageToPurchaseHistoryRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamConfigurations_ExamConfig~",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.RenameColumn(
                name: "ExamConfigId",
                table: "ModelExamPurchaseHistory",
                newName: "ModelExamPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamPurchaseHistory_ExamConfigId",
                table: "ModelExamPurchaseHistory",
                newName: "IX_ModelExamPurchaseHistory_ModelExamPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamPackages_ModelExamPackage~",
                table: "ModelExamPurchaseHistory",
                column: "ModelExamPackageId",
                principalTable: "ModelExamPackages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamPackages_ModelExamPackage~",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.RenameColumn(
                name: "ModelExamPackageId",
                table: "ModelExamPurchaseHistory",
                newName: "ExamConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamPurchaseHistory_ModelExamPackageId",
                table: "ModelExamPurchaseHistory",
                newName: "IX_ModelExamPurchaseHistory_ExamConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamConfigurations_ExamConfig~",
                table: "ModelExamPurchaseHistory",
                column: "ExamConfigId",
                principalTable: "ModelExamConfigurations",
                principalColumn: "Id");
        }
    }
}
