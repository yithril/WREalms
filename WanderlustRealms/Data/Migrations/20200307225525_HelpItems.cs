using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class HelpItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Living",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HelpItems",
                columns: table => new
                {
                    HelpItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HelpTerm = table.Column<string>(nullable: false),
                    HelpDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpItems", x => x.HelpItemID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpItems");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Living");
        }
    }
}
