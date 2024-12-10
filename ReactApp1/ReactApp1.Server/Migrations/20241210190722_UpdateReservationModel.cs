using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropColumn(
                name: "CustomerCount",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ReservedSpot",
                table: "Reservation");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByEmployeeId",
                table: "Reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhoneNumber",
                table: "Reservation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "GiftCard",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CreatedByEmployeeId",
                table: "Reservation",
                column: "CreatedByEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Table",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Employee_CreatedByEmployeeId",
                table: "Reservation",
                column: "CreatedByEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Employee_CreatedByEmployeeId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_CreatedByEmployeeId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CreatedByEmployeeId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CustomerPhoneNumber",
                table: "Reservation");

            migrationBuilder.AddColumn<int>(
                name: "CustomerCount",
                table: "Reservation",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservedSpot",
                table: "Reservation",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

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
