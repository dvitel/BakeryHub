using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class FeedbackSubscription_Removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackSubscription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedbackSubscription",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackSubscription", x => new { x.UserId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_FeedbackSubscription_Contacts_UserId_ContactId",
                        columns: x => new { x.UserId, x.ContactId },
                        principalTable: "Contacts",
                        principalColumns: new[] { "UserId", "ContactId" },
                        onDelete: ReferentialAction.Restrict);
                });
        }
    }
}
