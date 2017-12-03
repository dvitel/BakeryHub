using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class NotificationLogKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationLog",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "NotificationLog");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "NotificationLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationLog",
                table: "NotificationLog",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLog_UserId_ContactId",
                table: "NotificationLog",
                columns: new[] { "UserId", "ContactId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationLog",
                table: "NotificationLog");

            migrationBuilder.DropIndex(
                name: "IX_NotificationLog_UserId_ContactId",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "NotificationLog");

            migrationBuilder.AddColumn<long>(
                name: "MessageId",
                table: "NotificationLog",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationLog",
                table: "NotificationLog",
                columns: new[] { "UserId", "ContactId", "MessageId" });
        }
    }
}
