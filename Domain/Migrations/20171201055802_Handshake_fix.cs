using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class Handshake_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Handshake_SupplierId",
                table: "Handshake",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Handshake_Customers_CustomerId",
                table: "Handshake",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Handshake_Suppliers_SupplierId",
                table: "Handshake",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Handshake_Orders_CustomerId_OrderId",
                table: "Handshake",
                columns: new[] { "CustomerId", "OrderId" },
                principalTable: "Orders",
                principalColumns: new[] { "CustomerId", "OrderId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Handshake_Customers_CustomerId",
                table: "Handshake");

            migrationBuilder.DropForeignKey(
                name: "FK_Handshake_Suppliers_SupplierId",
                table: "Handshake");

            migrationBuilder.DropForeignKey(
                name: "FK_Handshake_Orders_CustomerId_OrderId",
                table: "Handshake");

            migrationBuilder.DropIndex(
                name: "IX_Handshake_SupplierId",
                table: "Handshake");
        }
    }
}
