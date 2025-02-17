using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactUsCityColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ContactUsRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "NA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ContactUsRequests");
        }
    }
}
