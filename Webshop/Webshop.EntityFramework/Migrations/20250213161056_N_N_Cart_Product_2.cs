using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class N_N_Cart_Product_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageData_Carts_CartId",
                table: "StorageData");

            migrationBuilder.DropIndex(
                name: "IX_StorageData_CartId",
                table: "StorageData");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "StorageData");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quanity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => new { x.CartId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_StorageData_ProductId",
                        column: x => x.ProductId,
                        principalTable: "StorageData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "StorageData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StorageData_CartId",
                table: "StorageData",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageData_Carts_CartId",
                table: "StorageData",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
