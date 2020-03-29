using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class moarChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrMsg",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "HubFunction",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "GameActions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrMsg",
                table: "GameActions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HubFunction",
                table: "GameActions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "GameActions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
