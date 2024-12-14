using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class HistoricalDataForOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AlcoholicBeverage",
                table: "FullOrder",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "FullOrder",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByEmployeeId",
                table: "FullOrder",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FullOrder",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveTime",
                table: "FullOrder",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "FullOrder",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullOrder_CreatedByEmployeeId",
                table: "FullOrder",
                column: "CreatedByEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FullOrder_Employee_CreatedByEmployeeId",
                table: "FullOrder",
                column: "CreatedByEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullOrder_Employee_CreatedByEmployeeId",
                table: "FullOrder");

            migrationBuilder.DropIndex(
                name: "IX_FullOrder_CreatedByEmployeeId",
                table: "FullOrder");

            migrationBuilder.DropColumn(
                name: "AlcoholicBeverage",
                table: "FullOrder");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "FullOrder");

            migrationBuilder.DropColumn(
                name: "CreatedByEmployeeId",
                table: "FullOrder");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FullOrder");

            migrationBuilder.DropColumn(
                name: "ReceiveTime",
                table: "FullOrder");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "FullOrder");
        }
    }
}
