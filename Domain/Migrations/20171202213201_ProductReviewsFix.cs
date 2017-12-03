using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class ProductReviewsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Reviews_ReviewId",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Products_SupplierId_ProductId",
                table: "ProductReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview");

            migrationBuilder.RenameTable(
                name: "ProductReview",
                newName: "ProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_SupplierId_ProductId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_SupplierId_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Reviews_ReviewId",
                table: "ProductReviews",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_SupplierId_ProductId",
                table: "ProductReviews",
                columns: new[] { "SupplierId", "ProductId" },
                principalTable: "Products",
                principalColumns: new[] { "SupplierId", "ProductId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Reviews_ReviewId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_SupplierId_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.RenameTable(
                name: "ProductReviews",
                newName: "ProductReview");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_SupplierId_ProductId",
                table: "ProductReview",
                newName: "IX_ProductReview_SupplierId_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Reviews_ReviewId",
                table: "ProductReview",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Products_SupplierId_ProductId",
                table: "ProductReview",
                columns: new[] { "SupplierId", "ProductId" },
                principalTable: "Products",
                principalColumns: new[] { "SupplierId", "ProductId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
