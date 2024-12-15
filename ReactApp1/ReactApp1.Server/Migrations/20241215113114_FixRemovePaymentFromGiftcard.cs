using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixRemovePaymentFromGiftcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Payment_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropIndex(
                name: "IX_GiftCard_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "GiftCard");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_GiftCard_GiftCardId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_GiftCardId",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "GiftCard",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCard_PaymentId",
                table: "GiftCard",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Payment_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "PaymentId");
        }
    }
}
