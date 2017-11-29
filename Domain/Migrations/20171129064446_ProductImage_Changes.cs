using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class ProductImage_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "LogicalPath",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogicalPath",
                table: "ProductImages");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "ProductImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ProductImages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "ProductImages",
                nullable: false,
                defaultValue: 0);
        }
    }
}
