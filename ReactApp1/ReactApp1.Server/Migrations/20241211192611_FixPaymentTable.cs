using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_Order_OrderId",
                table: "Table");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Table",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "TipFixed",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "TipPercentage",
                table: "Table");

            migrationBuilder.RenameTable(
                name: "Table",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Table_OrderId",
                table: "Payment",
                newName: "IX_Payment_OrderId");

            migrationBuilder.AddColumn<decimal>(
                name: "TipFixed",
                table: "Order",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipPercentage",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Payment",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCard_Code",
                table: "GiftCard",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Payment_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Payment_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_GiftCard_Code",
                table: "GiftCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "TipFixed",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TipPercentage",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Table");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_OrderId",
                table: "Table",
                newName: "IX_Table_OrderId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table",
                table: "Table",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Table",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Order_OrderId",
                table: "Table",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
