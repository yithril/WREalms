using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class changeanotherthing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHaggleOn",
                table: "Living",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHaggleOn",
                table: "Living");
        }
    }
}
