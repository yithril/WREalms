using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class LivingMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerQuests_Living_PlayerCharacterID",
                table: "PlayerQuests");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSkills_Living_PlayerCharacterID",
                table: "PlayerSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerSkills",
                table: "PlayerSkills");

            migrationBuilder.DropIndex(
                name: "IX_PlayerSkills_PlayerCharacterID",
                table: "PlayerSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerQuests",
                table: "PlayerQuests");

            migrationBuilder.DropColumn(
                name: "PlayerCharacterID",
                table: "PlayerSkills");

            migrationBuilder.DropColumn(
                name: "PlayerCharacterID",
                table: "PlayerQuests");

            migrationBuilder.DropColumn(
                name: "PlayerCharacterID",
                table: "Living");

            migrationBuilder.AddColumn<int>(
                name: "LivingID",
                table: "PlayerSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerCharacterLivingID",
                table: "PlayerSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivingID",
                table: "PlayerQuests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerCharacterLivingID",
                table: "PlayerQuests",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerSkills",
                table: "PlayerSkills",
                columns: new[] { "SkillID", "LivingID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerQuests",
                table: "PlayerQuests",
                columns: new[] { "LivingID", "QuestID" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSkills_LivingID",
                table: "PlayerSkills",
                column: "LivingID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSkills_PlayerCharacterLivingID",
                table: "PlayerSkills",
                column: "PlayerCharacterLivingID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerQuests_PlayerCharacterLivingID",
                table: "PlayerQuests",
                column: "PlayerCharacterLivingID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerQuests_Living_LivingID",
                table: "PlayerQuests",
                column: "LivingID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerQuests_Living_PlayerCharacterLivingID",
                table: "PlayerQuests",
                column: "PlayerCharacterLivingID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSkills_Living_LivingID",
                table: "PlayerSkills",
                column: "LivingID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSkills_Living_PlayerCharacterLivingID",
                table: "PlayerSkills",
                column: "PlayerCharacterLivingID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerQuests_Living_LivingID",
                table: "PlayerQuests");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerQuests_Living_PlayerCharacterLivingID",
                table: "PlayerQuests");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSkills_Living_LivingID",
                table: "PlayerSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSkills_Living_PlayerCharacterLivingID",
                table: "PlayerSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerSkills",
                table: "PlayerSkills");

            migrationBuilder.DropIndex(
                name: "IX_PlayerSkills_LivingID",
                table: "PlayerSkills");

            migrationBuilder.DropIndex(
                name: "IX_PlayerSkills_PlayerCharacterLivingID",
                table: "PlayerSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerQuests",
                table: "PlayerQuests");

            migrationBuilder.DropIndex(
                name: "IX_PlayerQuests_PlayerCharacterLivingID",
                table: "PlayerQuests");

            migrationBuilder.DropColumn(
                name: "LivingID",
                table: "PlayerSkills");

            migrationBuilder.DropColumn(
                name: "PlayerCharacterLivingID",
                table: "PlayerSkills");

            migrationBuilder.DropColumn(
                name: "LivingID",
                table: "PlayerQuests");

            migrationBuilder.DropColumn(
                name: "PlayerCharacterLivingID",
                table: "PlayerQuests");

            migrationBuilder.AddColumn<int>(
                name: "PlayerCharacterID",
                table: "PlayerSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerCharacterID",
                table: "PlayerQuests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerCharacterID",
                table: "Living",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerSkills",
                table: "PlayerSkills",
                columns: new[] { "SkillID", "PlayerCharacterID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerQuests",
                table: "PlayerQuests",
                columns: new[] { "PlayerCharacterID", "QuestID" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSkills_PlayerCharacterID",
                table: "PlayerSkills",
                column: "PlayerCharacterID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerQuests_Living_PlayerCharacterID",
                table: "PlayerQuests",
                column: "PlayerCharacterID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSkills_Living_PlayerCharacterID",
                table: "PlayerSkills",
                column: "PlayerCharacterID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
