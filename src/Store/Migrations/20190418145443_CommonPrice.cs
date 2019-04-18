using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Migrations
{
    public partial class CommonPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommonPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonPrice",
                table: "Orders");
        }
    }
}
