using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OtpHistoryExpiryColAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete from ""OtpHistory"";");
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "OtpHistory");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiresOn",
                table: "OtpHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "NextOtpAfter",
                table: "OtpHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesRequested",
                table: "OtpHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OtpHistory_UserName",
                table: "OtpHistory",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OtpHistory_UserName",
                table: "OtpHistory");

            migrationBuilder.DropColumn(
                name: "ExpiresOn",
                table: "OtpHistory");

            migrationBuilder.DropColumn(
                name: "NextOtpAfter",
                table: "OtpHistory");

            migrationBuilder.DropColumn(
                name: "TimesRequested",
                table: "OtpHistory");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "OtpHistory",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
