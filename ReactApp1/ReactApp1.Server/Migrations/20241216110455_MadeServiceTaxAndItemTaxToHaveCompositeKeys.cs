using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class MadeServiceTaxAndItemTaxToHaveCompositeKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceTax",
                table: "ServiceTax");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTax_ServiceId",
                table: "ServiceTax");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemTax",
                table: "ItemTax");

            migrationBuilder.DropIndex(
                name: "IX_ItemTax_ItemId",
                table: "ItemTax");

            migrationBuilder.DropColumn(
                name: "ServiceTaxId",
                table: "ServiceTax");

            migrationBuilder.DropColumn(
                name: "ItemTaxId",
                table: "ItemTax");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceTax",
                table: "ServiceTax",
                columns: new[] { "ServiceId", "TaxId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemTax",
                table: "ItemTax",
                columns: new[] { "ItemId", "TaxId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceTax",
                table: "ServiceTax");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemTax",
                table: "ItemTax");

            migrationBuilder.AddColumn<int>(
                name: "ServiceTaxId",
                table: "ServiceTax",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ItemTaxId",
                table: "ItemTax",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceTax",
                table: "ServiceTax",
                column: "ServiceTaxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemTax",
                table: "ItemTax",
                column: "ItemTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTax_ServiceId",
                table: "ServiceTax",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTax_ItemId",
                table: "ItemTax",
                column: "ItemId");
        }
    }
}
