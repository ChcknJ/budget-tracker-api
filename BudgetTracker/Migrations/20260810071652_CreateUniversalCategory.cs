using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetTracker.Migrations
{
    /// <inheritdoc />
    public partial class CreateUniversalCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name", "user_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Food", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Transportation", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Utilities", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Entertainment", null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Shopping", null },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Health", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));
        }
    }
}
