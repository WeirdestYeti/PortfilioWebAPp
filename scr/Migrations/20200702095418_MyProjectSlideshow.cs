using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioWebApp.Migrations
{
    public partial class MyProjectSlideshow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowSlideshow",
                table: "MyProjects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowSlideshow",
                table: "MyProjects");
        }
    }
}
