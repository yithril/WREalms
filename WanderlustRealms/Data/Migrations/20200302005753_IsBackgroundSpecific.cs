using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class IsBackgroundSpecific : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBackgroundSpecific",
                table: "Skills",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBackgroundSpecific",
                table: "Skills");
        }
    }
}
