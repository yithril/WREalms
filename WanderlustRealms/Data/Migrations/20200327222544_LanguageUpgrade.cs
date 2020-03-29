using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class LanguageUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LivingID",
                table: "Quests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NPCLivingID",
                table: "Quests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestPoints",
                table: "Quests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QuestStartDialogue",
                table: "Quests",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "PlayerQuests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MainLanguageID",
                table: "Living",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPToSpend",
                table: "Living",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Adjective = table.Column<string>(nullable: true),
                    Adjective2 = table.Column<string>(nullable: true),
                    LanguageModifier = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageID);
                });

            migrationBuilder.CreateTable(
                name: "TrainerPreReqs",
                columns: table => new
                {
                    TrainerPreReqID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivingID = table.Column<int>(nullable: false),
                    NPCLivingID = table.Column<int>(nullable: true),
                    QuestID = table.Column<int>(nullable: true),
                    RaceID = table.Column<int>(nullable: true),
                    QuestPoints = table.Column<int>(nullable: true),
                    PlayerBackgroundID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerPreReqs", x => x.TrainerPreReqID);
                    table.ForeignKey(
                        name: "FK_TrainerPreReqs_Living_NPCLivingID",
                        column: x => x.NPCLivingID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerPreReqs_PlayerBackgrounds_PlayerBackgroundID",
                        column: x => x.PlayerBackgroundID,
                        principalTable: "PlayerBackgrounds",
                        principalColumn: "PlayerBackgroundID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerPreReqs_Quests_QuestID",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerPreReqs_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerSkills",
                columns: table => new
                {
                    TrainerSkillID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivingID = table.Column<int>(nullable: false),
                    NPCLivingID = table.Column<int>(nullable: true),
                    SkillID = table.Column<int>(nullable: false),
                    MinLevel = table.Column<int>(nullable: false),
                    MaxLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerSkills", x => x.TrainerSkillID);
                    table.ForeignKey(
                        name: "FK_TrainerSkills_Living_NPCLivingID",
                        column: x => x.NPCLivingID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerSkills_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivingLanguages",
                columns: table => new
                {
                    LivingID = table.Column<int>(nullable: false),
                    LanguageID = table.Column<int>(nullable: false),
                    Level = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivingLanguages", x => new { x.LanguageID, x.LivingID });
                    table.ForeignKey(
                        name: "FK_LivingLanguages_Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "LanguageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivingLanguages_Living_LivingID",
                        column: x => x.LivingID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RaceLanguages",
                columns: table => new
                {
                    RaceID = table.Column<int>(nullable: false),
                    LanguageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceLanguages", x => new { x.LanguageID, x.RaceID });
                    table.ForeignKey(
                        name: "FK_RaceLanguages_Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "LanguageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceLanguages_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quests_NPCLivingID",
                table: "Quests",
                column: "NPCLivingID");

            migrationBuilder.CreateIndex(
                name: "IX_LivingLanguages_LivingID",
                table: "LivingLanguages",
                column: "LivingID");

            migrationBuilder.CreateIndex(
                name: "IX_RaceLanguages_RaceID",
                table: "RaceLanguages",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerPreReqs_NPCLivingID",
                table: "TrainerPreReqs",
                column: "NPCLivingID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerPreReqs_PlayerBackgroundID",
                table: "TrainerPreReqs",
                column: "PlayerBackgroundID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerPreReqs_QuestID",
                table: "TrainerPreReqs",
                column: "QuestID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerPreReqs_RaceID",
                table: "TrainerPreReqs",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSkills_NPCLivingID",
                table: "TrainerSkills",
                column: "NPCLivingID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSkills_SkillID",
                table: "TrainerSkills",
                column: "SkillID");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Living_NPCLivingID",
                table: "Quests",
                column: "NPCLivingID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Living_NPCLivingID",
                table: "Quests");

            migrationBuilder.DropTable(
                name: "LivingLanguages");

            migrationBuilder.DropTable(
                name: "RaceLanguages");

            migrationBuilder.DropTable(
                name: "TrainerPreReqs");

            migrationBuilder.DropTable(
                name: "TrainerSkills");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Quests_NPCLivingID",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "LivingID",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "NPCLivingID",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "QuestPoints",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "QuestStartDialogue",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "PlayerQuests");

            migrationBuilder.DropColumn(
                name: "MainLanguageID",
                table: "Living");

            migrationBuilder.DropColumn(
                name: "XPToSpend",
                table: "Living");
        }
    }
}
