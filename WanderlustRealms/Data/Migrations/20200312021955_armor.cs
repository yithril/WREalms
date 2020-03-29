using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class armor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArmorClass",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneHanded",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmorClass",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsOneHanded",
                table: "Items");
        }
    }
}
