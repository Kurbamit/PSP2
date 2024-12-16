using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFKFromTaxHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullOrderServiceTax_FullOrderService_FullOrderId",
                table: "FullOrderServiceTax");

            migrationBuilder.DropForeignKey(
                name: "FK_FullOrderTax_FullOrder_FullOrderId",
                table: "FullOrderTax");

            migrationBuilder.DropIndex(
                name: "IX_FullOrderTax_FullOrderId",
                table: "FullOrderTax");

            migrationBuilder.DropIndex(
                name: "IX_FullOrderServiceTax_FullOrderId",
                table: "FullOrderServiceTax");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FullOrderTax_FullOrderId",
                table: "FullOrderTax",
                column: "FullOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderServiceTax_FullOrderId",
                table: "FullOrderServiceTax",
                column: "FullOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FullOrderServiceTax_FullOrderService_FullOrderId",
                table: "FullOrderServiceTax",
                column: "FullOrderId",
                principalTable: "FullOrderService",
                principalColumn: "FullOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FullOrderTax_FullOrder_FullOrderId",
                table: "FullOrderTax",
                column: "FullOrderId",
                principalTable: "FullOrder",
                principalColumn: "FullOrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
