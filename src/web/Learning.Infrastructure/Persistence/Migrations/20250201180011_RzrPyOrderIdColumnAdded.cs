using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RzrPyOrderIdColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RzrpayOrderId",
                table: "ModelExamOrders",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RzrpayOrderId",
                table: "ModelExamOrders");
        }
    }
}
