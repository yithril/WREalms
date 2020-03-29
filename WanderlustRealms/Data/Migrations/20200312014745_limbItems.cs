using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class limbItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LimbItems",
                columns: table => new
                {
                    LivingID = table.Column<int>(nullable: false),
                    LimbID = table.Column<int>(nullable: false),
                    ItemID = table.Column<int>(nullable: false),
                    PlayerCharacterLivingID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimbItems", x => new { x.ItemID, x.LimbID, x.LivingID });
                    table.ForeignKey(
                        name: "FK_LimbItems_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimbItems_Limbs_LimbID",
                        column: x => x.LimbID,
                        principalTable: "Limbs",
                        principalColumn: "LimbID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimbItems_Living_PlayerCharacterLivingID",
                        column: x => x.PlayerCharacterLivingID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LimbItems_LimbID",
                table: "LimbItems",
                column: "LimbID");

            migrationBuilder.CreateIndex(
                name: "IX_LimbItems_PlayerCharacterLivingID",
                table: "LimbItems",
                column: "PlayerCharacterLivingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LimbItems");
        }
    }
}
