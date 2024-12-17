using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class ServiceAssignedEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedEmployeeId",
                table: "Service",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AssignedEmployeeId",
                table: "FullOrderService",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Service_AssignedEmployeeId",
                table: "Service",
                column: "AssignedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderService_AssignedEmployeeId",
                table: "FullOrderService",
                column: "AssignedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FullOrderService_Employee_AssignedEmployeeId",
                table: "FullOrderService",
                column: "AssignedEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Employee_AssignedEmployeeId",
                table: "Service",
                column: "AssignedEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullOrderService_Employee_AssignedEmployeeId",
                table: "FullOrderService");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Employee_AssignedEmployeeId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_AssignedEmployeeId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_FullOrderService_AssignedEmployeeId",
                table: "FullOrderService");

            migrationBuilder.DropColumn(
                name: "AssignedEmployeeId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "AssignedEmployeeId",
                table: "FullOrderService");
        }
    }
}
