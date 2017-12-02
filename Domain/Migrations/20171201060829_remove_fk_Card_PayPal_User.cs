using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class remove_fk_Card_PayPal_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardPaymentMethods_Users_UserId",
                table: "CardPaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_PayPalPaymentMethods_Users_UserId",
                table: "PayPalPaymentMethods");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_CardPaymentMethods_Users_UserId",
                table: "CardPaymentMethods",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayPalPaymentMethods_Users_UserId",
                table: "PayPalPaymentMethods",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
