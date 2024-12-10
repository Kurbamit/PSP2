using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipFixed",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "TipPercentage",
                table: "Table");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Table",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Table");

            migrationBuilder.AddColumn<decimal>(
                name: "TipFixed",
                table: "Table",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipPercentage",
                table: "Table",
                type: "integer",
                nullable: true);
        }
    }
}
