using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioWebApp.Migrations
{
    public partial class MyProjectThumbNail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "MyProjects",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "MyProjects");
        }
    }
}
