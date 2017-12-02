using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class ReportSubscription_KeyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportSubscription",
                table: "ReportSubscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportSubscription",
                table: "ReportSubscription",
                columns: new[] { "UserId", "ContactId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportSubscription",
                table: "ReportSubscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportSubscription",
                table: "ReportSubscription",
                columns: new[] { "UserId", "ContactId" });
        }
    }
}
