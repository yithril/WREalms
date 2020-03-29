using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class PlayerSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerSkills",
                columns: table => new
                {
                    SkillID = table.Column<int>(nullable: false),
                    PlayerCharacterID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSkills", x => new { x.SkillID, x.PlayerCharacterID });
                    table.ForeignKey(
                        name: "FK_PlayerSkills_Living_PlayerCharacterID",
                        column: x => x.PlayerCharacterID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSkills_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSkills_PlayerCharacterID",
                table: "PlayerSkills",
                column: "PlayerCharacterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerSkills");
        }
    }
}
