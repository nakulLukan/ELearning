using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CouponCodeTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CouponCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DiscountPercentage = table.Column<float>(type: "real", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CouponCreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CouponUsedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouponCodes_Code",
                table: "CouponCodes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponCodes");
        }
    }
}
