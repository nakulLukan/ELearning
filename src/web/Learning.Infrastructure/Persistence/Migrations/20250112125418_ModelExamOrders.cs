using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModelExamOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamPackages_ModelExamPackage~",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropIndex(
                name: "IX_ModelExamPurchaseHistory_ModelExamPackageId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropColumn(
                name: "ModelExamPackageId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "ModelExamPurchaseHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ModelExamOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelExamPackageId = table.Column<int>(type: "integer", nullable: false),
                    OrderedInitiatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderedCompletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelExamOrders_ModelExamPackages_ModelExamPackageId",
                        column: x => x.ModelExamPackageId,
                        principalTable: "ModelExamPackages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamPurchaseHistory_OrderId",
                table: "ModelExamPurchaseHistory",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamOrders_ModelExamPackageId",
                table: "ModelExamOrders",
                column: "ModelExamPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamOrders_UserId",
                table: "ModelExamOrders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamOrders_OrderId",
                table: "ModelExamPurchaseHistory",
                column: "OrderId",
                principalTable: "ModelExamOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamOrders_OrderId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropTable(
                name: "ModelExamOrders");

            migrationBuilder.DropIndex(
                name: "IX_ModelExamPurchaseHistory_OrderId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "ModelExamPurchaseHistory",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ModelExamPackageId",
                table: "ModelExamPurchaseHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ModelExamPurchaseHistory",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamPurchaseHistory_ModelExamPackageId",
                table: "ModelExamPurchaseHistory",
                column: "ModelExamPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamPackages_ModelExamPackage~",
                table: "ModelExamPurchaseHistory",
                column: "ModelExamPackageId",
                principalTable: "ModelExamPackages",
                principalColumn: "Id");
        }
    }
}
