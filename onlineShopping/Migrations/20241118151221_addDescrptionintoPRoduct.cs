using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlineShopping.Migrations
{
    /// <inheritdoc />
    public partial class addDescrptionintoPRoduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descrption",
                table: "products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descrption",
                table: "products");
        }
    }
}
