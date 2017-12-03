using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;

namespace BakeryHub.Domain.Migrations
{
    public partial class Init_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            this.Down(migrationBuilder);
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.Substring‌​(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("bin"));
            var dir =
                baseDir.Contains(@"\Domain")
                ? Path.Combine(baseDir, @"SQL\db_init\data\")
                : Path.Combine(baseDir, "..", "Domain", @"SQL\db_init\data\");
            var d = new DirectoryInfo(dir);
            foreach (var file in d.EnumerateFiles())
            {
                System.Console.WriteLine($"Init data from: {file.FullName}");
                migrationBuilder.Sql(File.ReadAllText(file.FullName));
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE Countries");
            migrationBuilder.Sql("DELETE States");
            migrationBuilder.Sql("DELETE ProductCategories");
        }
    }
}
