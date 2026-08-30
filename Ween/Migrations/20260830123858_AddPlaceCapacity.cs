using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ween.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaceCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 40);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Places");
        }
    }
}
