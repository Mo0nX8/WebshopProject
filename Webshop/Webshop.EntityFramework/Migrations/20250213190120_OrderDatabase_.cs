using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class OrderDatabase_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdersId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrdersId",
                table: "CartItems",
                column: "OrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrdersId",
                table: "CartItems",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrdersId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrdersId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "CartItems");
        }
    }
}
