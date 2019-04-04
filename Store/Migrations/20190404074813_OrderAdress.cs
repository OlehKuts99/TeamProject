using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Migrations
{
    public partial class OrderAdress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndPointCity",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndPointStreet",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPointCity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EndPointStreet",
                table: "Orders");
        }
    }
}
