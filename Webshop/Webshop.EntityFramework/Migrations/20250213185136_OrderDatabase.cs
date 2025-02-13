using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class OrderDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCount",
                table: "StorageData",
                newName: "Quanity");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StorageData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "StorageData");

            migrationBuilder.RenameColumn(
                name: "Quanity",
                table: "StorageData",
                newName: "ProductCount");
        }
    }
}
