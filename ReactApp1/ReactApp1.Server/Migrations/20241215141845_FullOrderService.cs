using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class FullOrderService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FullOrderServiceFullOrderId",
                table: "Service",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FullOrderService",
                columns: table => new
                {
                    FullOrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    ServiceLength = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric", nullable: true),
                    Tax = table.Column<decimal>(type: "numeric", nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByEmployeeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullOrderService", x => x.FullOrderId);
                    table.ForeignKey(
                        name: "FK_FullOrderService_Employee_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_FullOrderService_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_FullOrderServiceFullOrderId",
                table: "Service",
                column: "FullOrderServiceFullOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderService_CreatedByEmployeeId",
                table: "FullOrderService",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderService_OrderId",
                table: "FullOrderService",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_FullOrderService_FullOrderServiceFullOrderId",
                table: "Service",
                column: "FullOrderServiceFullOrderId",
                principalTable: "FullOrderService",
                principalColumn: "FullOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_FullOrderService_FullOrderServiceFullOrderId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "FullOrderService");

            migrationBuilder.DropIndex(
                name: "IX_Service_FullOrderServiceFullOrderId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "FullOrderServiceFullOrderId",
                table: "Service");
        }
    }
}
