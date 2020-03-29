using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class anotherQuickFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaceBackgroundID",
                table: "RaceBackgrounds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceBackgroundID",
                table: "RaceBackgrounds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
