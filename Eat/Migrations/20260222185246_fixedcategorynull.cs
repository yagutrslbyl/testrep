using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eat.Migrations
{
    /// <inheritdoc />
    public partial class fixedcategorynull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Categories_CategoryId",
                table: "Stories");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Stories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Categories_CategoryId",
                table: "Stories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Categories_CategoryId",
                table: "Stories");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Stories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Categories_CategoryId",
                table: "Stories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
