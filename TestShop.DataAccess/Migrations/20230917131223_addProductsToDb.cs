using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addProductsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price50 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price100 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDateTime", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 17, 18, 12, 22, 965, DateTimeKind.Local).AddTicks(2267), 1, "Action" },
                    { 2, new DateTime(2023, 9, 17, 18, 12, 22, 965, DateTimeKind.Local).AddTicks(2276), 2, "SciFi" },
                    { 3, new DateTime(2023, 9, 17, 18, 12, 22, 965, DateTimeKind.Local).AddTicks(2277), 3, "History" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Hunter S. Thompson", "A wild journey to the heart of the American Dream.", "978-0679785897", 19.99m, 15.99m, 10.99m, 12.99m, "Fear and Loathing in Las Vegas" },
                    { 2, "Harper Lee", "A classic novel about racial injustice and moral growth.", "978-0061120084", 14.99m, 12.49m, 9.99m, 10.99m, "To Kill a Mockingbird" },
                    { 3, "F. Scott Fitzgerald", "A story of wealth, love, and tragedy in the Jazz Age.", "978-0743273565", 12.99m, 10.99m, 8.99m, 9.49m, "The Great Gatsby" },
                    { 4, "George Orwell", "A dystopian novel depicting a totalitarian society.", "978-0451524935", 11.99m, 9.99m, 7.99m, 8.49m, "1984" },
                    { 5, "Jane Austen", "A timeless story of love and class in 19th-century England.", "978-0141439518", 10.99m, 8.99m, 6.99m, 7.49m, "Pride and Prejudice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
