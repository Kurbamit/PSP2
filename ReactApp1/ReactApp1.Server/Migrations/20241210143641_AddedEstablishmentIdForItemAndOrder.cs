using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedEstablishmentIdForItemAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByEmployeeId",
                table: "Item",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId",
                table: "Item",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "GiftCard",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EstablishmentId",
                table: "Order",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CreatedByEmployeeId",
                table: "Item",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_EstablishmentId",
                table: "Item",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Table",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Employee_CreatedByEmployeeId",
                table: "Item",
                column: "CreatedByEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Establishment_EstablishmentId",
                table: "Order",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "EstablishmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Employee_CreatedByEmployeeId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Establishment_EstablishmentId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_EstablishmentId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Item_CreatedByEmployeeId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_EstablishmentId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedByEmployeeId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "Item");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "GiftCard",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Table",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
