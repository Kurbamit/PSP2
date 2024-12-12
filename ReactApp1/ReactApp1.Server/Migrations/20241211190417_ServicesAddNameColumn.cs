using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class ServicesAddNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Service",
                newName: "ServiceId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Service",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Service");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Service",
                newName: "ReservationId");
        }
    }
}
