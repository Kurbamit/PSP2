using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Establishment",
                columns: table => new
                {
                    EstablishmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstablishmentAddressId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishment", x => x.EstablishmentId);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cost = table.Column<decimal>(type: "numeric", nullable: true),
                    Tax = table.Column<decimal>(type: "numeric", nullable: true),
                    AlcoholicBeverage = table.Column<bool>(type: "boolean", nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomerCount = table.Column<int>(type: "integer", nullable: true),
                    ReservedSpot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationId);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    GuidId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "Guid identification"),
                    IntId = table.Column<int>(type: "integer", nullable: false, comment: "Integer identification")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Timestamp when the record was created")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.GuidId);
                },
                comment: "Table for testing purposes");

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<int>(type: "integer", nullable: false),
                    EstablishmentId = table.Column<int>(type: "integer", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    PersonalCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Phone = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "EstablishmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentAddress",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    StreetNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EstablishmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentAddress", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_EstablishmentAddress_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "EstablishmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    StorageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstablishmentId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.StorageId);
                    table.ForeignKey(
                        name: "FK_Storage_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "EstablishmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storage_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddress",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    StreetNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    HouseNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddress", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_EmployeeAddress_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedByEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DiscountPercentage = table.Column<int>(type: "integer", nullable: true),
                    DiscountFixed = table.Column<decimal>(type: "numeric", nullable: true),
                    PaymentId = table.Column<int>(type: "integer", nullable: true),
                    Refunded = table.Column<bool>(type: "boolean", nullable: false),
                    ReservationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Employee_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "ReservationId");
                });

            migrationBuilder.CreateTable(
                name: "FullOrder",
                columns: table => new
                {
                    FullOrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullOrder", x => x.FullOrderId);
                    table.ForeignKey(
                        name: "FK_FullOrder_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FullOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TipPercentage = table.Column<int>(type: "integer", nullable: true),
                    TipFixed = table.Column<decimal>(type: "numeric", nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    GiftCardId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Table_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiftCard",
                columns: table => new
                {
                    GiftCardId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PaymentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftCard", x => x.GiftCardId);
                    table.ForeignKey(
                        name: "FK_GiftCard_Table_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Table",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EstablishmentId",
                table: "Employee",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddress_EmployeeId",
                table: "EmployeeAddress",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentAddress_EstablishmentId",
                table: "EstablishmentAddress",
                column: "EstablishmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullOrder_ItemId",
                table: "FullOrder",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FullOrder_OrderId",
                table: "FullOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCard_PaymentId",
                table: "GiftCard",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedByEmployeeId",
                table: "Order",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ReservationId",
                table: "Order",
                column: "ReservationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Storage_EstablishmentId",
                table: "Storage",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemId",
                table: "Storage",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Table_OrderId",
                table: "Table",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAddress");

            migrationBuilder.DropTable(
                name: "EstablishmentAddress");

            migrationBuilder.DropTable(
                name: "FullOrder");

            migrationBuilder.DropTable(
                name: "GiftCard");

            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Establishment");
        }
    }
}
