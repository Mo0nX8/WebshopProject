using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Google : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentOption",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingOption",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentOption",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingOption",
                table: "Orders");
        }
    }
}
