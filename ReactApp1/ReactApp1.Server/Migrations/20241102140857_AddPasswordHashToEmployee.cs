using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullOrder_Item_ItemId",
                table: "FullOrder");

            migrationBuilder.DropIndex(
                name: "IX_FullOrder_ItemId",
                table: "FullOrder");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Employee",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FullOrderItem",
                columns: table => new
                {
                    FullOrdersFullOrderId = table.Column<int>(type: "integer", nullable: false),
                    ItemsItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullOrderItem", x => new { x.FullOrdersFullOrderId, x.ItemsItemId });
                    table.ForeignKey(
                        name: "FK_FullOrderItem_FullOrder_FullOrdersFullOrderId",
                        column: x => x.FullOrdersFullOrderId,
                        principalTable: "FullOrder",
                        principalColumn: "FullOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FullOrderItem_Item_ItemsItemId",
                        column: x => x.ItemsItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FullOrderItem_ItemsItemId",
                table: "FullOrderItem",
                column: "ItemsItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FullOrderItem");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrder_ItemId",
                table: "FullOrder",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FullOrder_Item_ItemId",
                table: "FullOrder",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
