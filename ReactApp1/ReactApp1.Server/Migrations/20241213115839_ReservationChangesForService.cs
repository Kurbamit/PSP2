using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReservationChangesForService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstablishmentAddressId",
                table: "Reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId",
                table: "Reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_EstablishmentAddressId",
                table: "Reservation",
                column: "EstablishmentAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_EstablishmentId",
                table: "Reservation",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ServiceId",
                table: "Reservation",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_EstablishmentAddress_EstablishmentAddressId",
                table: "Reservation",
                column: "EstablishmentAddressId",
                principalTable: "EstablishmentAddress",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Establishment_EstablishmentId",
                table: "Reservation",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "EstablishmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Service_ServiceId",
                table: "Reservation",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_EstablishmentAddress_EstablishmentAddressId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Establishment_EstablishmentId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Service_ServiceId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_EstablishmentAddressId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_EstablishmentId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ServiceId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "EstablishmentAddressId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Reservation");
        }
    }
}
