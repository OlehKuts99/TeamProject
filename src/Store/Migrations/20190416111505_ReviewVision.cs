using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Migrations
{
    public partial class ReviewVision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleForAll",
                table: "Reviews",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisibleForAll",
                table: "Reviews");
        }
    }
}
