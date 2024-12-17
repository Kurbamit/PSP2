using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseToTrackTaxHistoricData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FullOrderServiceTax",
                columns: table => new
                {
                    FullOrderServiceTaxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullOrderId = table.Column<int>(type: "integer", nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullOrderServiceTax", x => x.FullOrderServiceTaxId);
                    table.ForeignKey(
                        name: "FK_FullOrderServiceTax_FullOrderService_FullOrderId",
                        column: x => x.FullOrderId,
                        principalTable: "FullOrderService",
                        principalColumn: "FullOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FullOrderTax",
                columns: table => new
                {
                    FullOrderTaxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullOrderId = table.Column<int>(type: "integer", nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullOrderTax", x => x.FullOrderTaxId);
                    table.ForeignKey(
                        name: "FK_FullOrderTax_FullOrder_FullOrderId",
                        column: x => x.FullOrderId,
                        principalTable: "FullOrder",
                        principalColumn: "FullOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderServiceTax_FullOrderId",
                table: "FullOrderServiceTax",
                column: "FullOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderTax_FullOrderId",
                table: "FullOrderTax",
                column: "FullOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FullOrderServiceTax");

            migrationBuilder.DropTable(
                name: "FullOrderTax");
        }
    }
}
