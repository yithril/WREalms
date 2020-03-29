using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class ShopKEeper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    ShopID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(nullable: true),
                    LivingID = table.Column<int>(nullable: false),
                    NPCLivingID = table.Column<int>(nullable: true),
                    HaggleModifier = table.Column<int>(nullable: false),
                    PlayerBackgroundID = table.Column<int>(nullable: true),
                    RaceID = table.Column<int>(nullable: true),
                    RoomKingdomID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.ShopID);
                    table.ForeignKey(
                        name: "FK_Shops_Living_NPCLivingID",
                        column: x => x.NPCLivingID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shops_PlayerBackgrounds_PlayerBackgroundID",
                        column: x => x.PlayerBackgroundID,
                        principalTable: "PlayerBackgrounds",
                        principalColumn: "PlayerBackgroundID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shops_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shops_RoomKingdoms_RoomKingdomID",
                        column: x => x.RoomKingdomID,
                        principalTable: "RoomKingdoms",
                        principalColumn: "RoomKingdomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    ShopItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(nullable: false),
                    ShopID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.ShopItemID);
                    table.ForeignKey(
                        name: "FK_ShopItems_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItems_Shops_ShopID",
                        column: x => x.ShopID,
                        principalTable: "Shops",
                        principalColumn: "ShopID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemID",
                table: "ShopItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ShopID",
                table: "ShopItems",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_NPCLivingID",
                table: "Shops",
                column: "NPCLivingID");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_PlayerBackgroundID",
                table: "Shops",
                column: "PlayerBackgroundID");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_RaceID",
                table: "Shops",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_RoomKingdomID",
                table: "Shops",
                column: "RoomKingdomID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "Shops");
        }
    }
}
