using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioWebApp.Migrations
{
    public partial class MyProjectLangAndTechFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherTechnologies",
                table: "MyProjects",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsedLanguages",
                table: "MyProjects",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherTechnologies",
                table: "MyProjects");

            migrationBuilder.DropColumn(
                name: "UsedLanguages",
                table: "MyProjects");
        }
    }
}
