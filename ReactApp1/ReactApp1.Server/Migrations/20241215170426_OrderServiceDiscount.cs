using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class OrderServiceDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "FullOrderService",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderService_DiscountId",
                table: "FullOrderService",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_FullOrderService_Discount_DiscountId",
                table: "FullOrderService",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullOrderService_Discount_DiscountId",
                table: "FullOrderService");

            migrationBuilder.DropIndex(
                name: "IX_FullOrderService_DiscountId",
                table: "FullOrderService");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "FullOrderService");
        }
    }
}
