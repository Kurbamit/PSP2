using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemadeTaxSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "FullOrderService");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "FullOrder");

            migrationBuilder.CreateTable(
                name: "Tax",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tax", x => x.TaxId);
                });

            migrationBuilder.CreateTable(
                name: "ItemTax",
                columns: table => new
                {
                    ItemTaxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    TaxId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTax", x => x.ItemTaxId);
                    table.ForeignKey(
                        name: "FK_ItemTax_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTax_Tax_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Tax",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTax",
                columns: table => new
                {
                    ServiceTaxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    TaxId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTax", x => x.ServiceTaxId);
                    table.ForeignKey(
                        name: "FK_ServiceTax_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTax_Tax_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Tax",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemTax_ItemId",
                table: "ItemTax",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTax_TaxId",
                table: "ItemTax",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTax_ServiceId",
                table: "ServiceTax",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTax_TaxId",
                table: "ServiceTax",
                column: "TaxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemTax");

            migrationBuilder.DropTable(
                name: "ServiceTax");

            migrationBuilder.DropTable(
                name: "Tax");

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Service",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Item",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "FullOrderService",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "FullOrder",
                type: "numeric",
                nullable: true);
        }
    }
}
