using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlineShopping.Migrations
{
    /// <inheritdoc />
    public partial class removeidforcartitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "cartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "cartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
