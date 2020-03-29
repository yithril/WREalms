using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class languages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StartingLevel",
                table: "RaceLanguages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingLevel",
                table: "RaceLanguages");
        }
    }
}
