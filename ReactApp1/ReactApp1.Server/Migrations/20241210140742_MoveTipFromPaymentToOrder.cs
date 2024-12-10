﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class MoveTipFromPaymentToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.AddColumn<decimal>(
                name: "TipFixed",
                table: "Order",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipPercentage",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "GiftCard",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Table",
                principalColumn: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard");

            migrationBuilder.DropColumn(
                name: "TipFixed",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TipPercentage",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "GiftCard",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Table_PaymentId",
                table: "GiftCard",
                column: "PaymentId",
                principalTable: "Table",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
