using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class LivingAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Living",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Living");
        }
    }
}
