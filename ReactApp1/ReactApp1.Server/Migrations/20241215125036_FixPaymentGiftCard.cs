using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixPaymentGiftCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_GiftCard_GiftCardId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_GiftCardId",
                table: "Payment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payment_GiftCardId",
                table: "Payment",
                column: "GiftCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_GiftCard_GiftCardId",
                table: "Payment",
                column: "GiftCardId",
                principalTable: "GiftCard",
                principalColumn: "GiftCardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
