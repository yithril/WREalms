using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class limbItemFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LimbItems_Living_PlayerCharacterLivingID",
                table: "LimbItems");

            migrationBuilder.DropIndex(
                name: "IX_LimbItems_PlayerCharacterLivingID",
                table: "LimbItems");

            migrationBuilder.DropColumn(
                name: "PlayerCharacterLivingID",
                table: "LimbItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerCharacterLivingID",
                table: "LimbItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LimbItems_PlayerCharacterLivingID",
                table: "LimbItems",
                column: "PlayerCharacterLivingID");

            migrationBuilder.AddForeignKey(
                name: "FK_LimbItems_Living_PlayerCharacterLivingID",
                table: "LimbItems",
                column: "PlayerCharacterLivingID",
                principalTable: "Living",
                principalColumn: "LivingID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
