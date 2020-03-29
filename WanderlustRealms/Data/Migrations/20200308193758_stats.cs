using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class stats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Level",
                table: "PlayerSkills",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Charisma",
                table: "Living",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity",
                table: "Living",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Durability",
                table: "Living",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intellect",
                table: "Living",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intuition",
                table: "Living",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Willpower",
                table: "Living",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "PlayerSkills");

            migrationBuilder.DropColumn(
                name: "Charisma",
                table: "Living");

            migrationBuilder.DropColumn(
                name: "Dexterity",
                table: "Living");

            migrationBuilder.DropColumn(
                name: "Durability",
                table: "Living");

            migrationBuilder.DropColumn(
                name: "Intellect",
                table: "Living");

            migrationBuilder.DropColumn(
                name: "Intuition",
                table: "Living");

            migrationBuilder.DropColumn(
                name: "Willpower",
                table: "Living");
        }
    }
}
