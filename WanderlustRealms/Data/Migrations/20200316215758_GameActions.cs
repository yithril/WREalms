using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class GameActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameActions",
                columns: table => new
                {
                    GameActionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Verb = table.Column<string>(nullable: false),
                    CommandLength = table.Column<int>(nullable: false),
                    IsStandard = table.Column<bool>(nullable: false),
                    AlternateKeywords = table.Column<string>(nullable: true),
                    Service = table.Column<string>(nullable: false),
                    Function = table.Column<string>(nullable: false),
                    ErrMsg = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameActions", x => x.GameActionID);
                });

            migrationBuilder.CreateTable(
                name: "GameActionJoins",
                columns: table => new
                {
                    GameActionID = table.Column<int>(nullable: false),
                    LivingID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameActionJoins", x => new { x.GameActionID, x.LivingID });
                    table.ForeignKey(
                        name: "FK_GameActionJoins_GameActions_GameActionID",
                        column: x => x.GameActionID,
                        principalTable: "GameActions",
                        principalColumn: "GameActionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameActionJoins_Living_LivingID",
                        column: x => x.LivingID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameActionJoins_LivingID",
                table: "GameActionJoins",
                column: "LivingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameActionJoins");

            migrationBuilder.DropTable(
                name: "GameActions");
        }
    }
}
