using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlineShopping.Migrations
{
    /// <inheritdoc />
    public partial class addcoupontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoupnId",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    CouponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.CouponId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_CoupnId",
                table: "products",
                column: "CoupnId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_coupons_CoupnId",
                table: "products",
                column: "CoupnId",
                principalTable: "coupons",
                principalColumn: "CouponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_coupons_CoupnId",
                table: "products");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropIndex(
                name: "IX_products_CoupnId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "CoupnId",
                table: "products");
        }
    }
}
