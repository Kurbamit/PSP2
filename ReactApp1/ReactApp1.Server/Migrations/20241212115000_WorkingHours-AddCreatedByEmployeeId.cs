using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class WorkingHoursAddCreatedByEmployeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByEmployeeId",
                table: "WorkingHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_CreatedByEmployeeId",
                table: "WorkingHours",
                column: "CreatedByEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHours_Employee_CreatedByEmployeeId",
                table: "WorkingHours",
                column: "CreatedByEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHours_Employee_CreatedByEmployeeId",
                table: "WorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHours_CreatedByEmployeeId",
                table: "WorkingHours");

            migrationBuilder.DropColumn(
                name: "CreatedByEmployeeId",
                table: "WorkingHours");
        }
    }
}
