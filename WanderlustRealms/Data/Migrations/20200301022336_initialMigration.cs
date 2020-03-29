using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WanderlustRealms.Data.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Bodies",
                columns: table => new
                {
                    BodyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    BodyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodies", x => x.BodyID);
                });

            migrationBuilder.CreateTable(
                name: "Limbs",
                columns: table => new
                {
                    LimbID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: false),
                    IsVital = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsWieldable = table.Column<bool>(nullable: false),
                    CanFly = table.Column<bool>(nullable: false),
                    IsEnhancedUnarmed = table.Column<bool>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limbs", x => x.LimbID);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBackgrounds",
                columns: table => new
                {
                    PlayerBackgroundID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PicURL = table.Column<string>(nullable: true),
                    IsPlayable = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBackgrounds", x => x.PlayerBackgroundID);
                });

            migrationBuilder.CreateTable(
                name: "RoomKingdoms",
                columns: table => new
                {
                    RoomKingdomID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomKingdoms", x => x.RoomKingdomID);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PicURL = table.Column<string>(nullable: true),
                    RelatedStat = table.Column<int>(nullable: false),
                    SecondaryStat = table.Column<int>(nullable: true),
                    BaseLearnRate = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MinGoodAlignment = table.Column<int>(nullable: true),
                    MaxGoodAlignment = table.Column<int>(nullable: true),
                    MinOrderAlignment = table.Column<int>(nullable: true),
                    MaxOrderAlignment = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillID);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    RaceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    MinLightLevel = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    IsPremium = table.Column<bool>(nullable: false),
                    IsPlayable = table.Column<bool>(nullable: false),
                    StatPoints = table.Column<int>(nullable: false),
                    SkillPoints = table.Column<int>(nullable: false),
                    PicURL = table.Column<string>(nullable: true),
                    BodyID = table.Column<int>(nullable: false),
                    MinInt = table.Column<int>(nullable: false),
                    MaxInt = table.Column<int>(nullable: false),
                    MaxDur = table.Column<int>(nullable: false),
                    MinDur = table.Column<int>(nullable: false),
                    MinIntuit = table.Column<int>(nullable: false),
                    MaxIntuit = table.Column<int>(nullable: false),
                    MinDex = table.Column<int>(nullable: false),
                    MaxDex = table.Column<int>(nullable: false),
                    MinWill = table.Column<int>(nullable: false),
                    MaxWill = table.Column<int>(nullable: false),
                    MinCha = table.Column<int>(nullable: false),
                    MaxCha = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.RaceID);
                    table.ForeignKey(
                        name: "FK_Races_Bodies_BodyID",
                        column: x => x.BodyID,
                        principalTable: "Bodies",
                        principalColumn: "BodyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BodyLimbJoins",
                columns: table => new
                {
                    BodyID = table.Column<int>(nullable: false),
                    LimbID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyLimbJoins", x => new { x.LimbID, x.BodyID });
                    table.ForeignKey(
                        name: "FK_BodyLimbJoins_Bodies_BodyID",
                        column: x => x.BodyID,
                        principalTable: "Bodies",
                        principalColumn: "BodyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BodyLimbJoins_Limbs_LimbID",
                        column: x => x.LimbID,
                        principalTable: "Limbs",
                        principalColumn: "LimbID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomAreas",
                columns: table => new
                {
                    RoomAreaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: true),
                    MinLevel = table.Column<int>(nullable: false),
                    MaxLevel = table.Column<int>(nullable: true),
                    IsPremium = table.Column<bool>(nullable: false, defaultValue: false),
                    RoomKingdomID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAreas", x => x.RoomAreaID);
                    table.ForeignKey(
                        name: "FK_RoomAreas_RoomKingdoms_RoomKingdomID",
                        column: x => x.RoomKingdomID,
                        principalTable: "RoomKingdoms",
                        principalColumn: "RoomKingdomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundSkills",
                columns: table => new
                {
                    SkillID = table.Column<int>(nullable: false),
                    PlayerBackgroundID = table.Column<int>(nullable: false),
                    BackgroundSkillID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundSkills", x => new { x.SkillID, x.PlayerBackgroundID });
                    table.ForeignKey(
                        name: "FK_BackgroundSkills_PlayerBackgrounds_PlayerBackgroundID",
                        column: x => x.PlayerBackgroundID,
                        principalTable: "PlayerBackgrounds",
                        principalColumn: "PlayerBackgroundID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackgroundSkills_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: false),
                    ShortDesc = table.Column<string>(nullable: false),
                    LongDesc = table.Column<string>(nullable: false),
                    HowItWorksDesc = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    CanTake = table.Column<bool>(nullable: false, defaultValue: true),
                    Weight = table.Column<int>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    IsMagical = table.Column<bool>(nullable: false, defaultValue: false),
                    IsCursed = table.Column<bool>(nullable: false, defaultValue: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ArmorPoints = table.Column<int>(nullable: true),
                    IsPrimal = table.Column<bool>(nullable: true),
                    LimbIDList = table.Column<string>(nullable: true),
                    SkillBoost = table.Column<int>(nullable: true),
                    StatBoost = table.Column<int>(nullable: true),
                    StatToBoost = table.Column<int>(nullable: true),
                    SkillID = table.Column<int>(nullable: true),
                    IsAccessory = table.Column<bool>(nullable: true, defaultValue: false),
                    ItemType = table.Column<int>(nullable: true),
                    DamageType = table.Column<int>(nullable: true),
                    Coefficient = table.Column<int>(nullable: true),
                    DamageDice = table.Column<int>(nullable: true),
                    DamageConstant = table.Column<int>(nullable: true),
                    IsArtifact = table.Column<bool>(nullable: true),
                    Weapon_IsPrimal = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Items_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    QuestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    MinLevel = table.Column<int>(nullable: false),
                    MaxLevel = table.Column<int>(nullable: false),
                    GoldReward = table.Column<int>(nullable: false),
                    XPReward = table.Column<int>(nullable: false),
                    SkillID = table.Column<int>(nullable: true),
                    SkillBonusPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.QuestID);
                    table.ForeignKey(
                        name: "FK_Quests_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Living",
                columns: table => new
                {
                    LivingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(nullable: false),
                    ShortDesc = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    RaceID = table.Column<int>(nullable: false),
                    GoodAlignment = table.Column<int>(nullable: false),
                    OrderAlignment = table.Column<int>(nullable: false),
                    MaxHP = table.Column<int>(nullable: false),
                    MaxMP = table.Column<int>(nullable: false),
                    CurrentHP = table.Column<int>(nullable: false),
                    CurrentMP = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    CanWander = table.Column<bool>(nullable: true, defaultValue: false),
                    IsAggressive = table.Column<bool>(nullable: true, defaultValue: false),
                    IsShopKeep = table.Column<bool>(nullable: true, defaultValue: false),
                    IsUnique = table.Column<bool>(nullable: true),
                    IsQuestNPC = table.Column<bool>(nullable: true),
                    PlayerCharacterID = table.Column<int>(nullable: true),
                    UserID = table.Column<string>(nullable: true),
                    PlayerBackgroundID = table.Column<int>(nullable: true),
                    XP = table.Column<int>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    RoomKingdomID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Living", x => x.LivingID);
                    table.ForeignKey(
                        name: "FK_Living_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Living_PlayerBackgrounds_PlayerBackgroundID",
                        column: x => x.PlayerBackgroundID,
                        principalTable: "PlayerBackgrounds",
                        principalColumn: "PlayerBackgroundID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Living_RoomKingdoms_RoomKingdomID",
                        column: x => x.RoomKingdomID,
                        principalTable: "RoomKingdoms",
                        principalColumn: "RoomKingdomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Living_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RaceBackgrounds",
                columns: table => new
                {
                    RaceID = table.Column<int>(nullable: false),
                    PlayerBackgroundID = table.Column<int>(nullable: false),
                    RaceBackgroundID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceBackgrounds", x => new { x.RaceID, x.PlayerBackgroundID });
                    table.ForeignKey(
                        name: "FK_RaceBackgrounds_PlayerBackgrounds_PlayerBackgroundID",
                        column: x => x.PlayerBackgroundID,
                        principalTable: "PlayerBackgrounds",
                        principalColumn: "PlayerBackgroundID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceBackgrounds_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceSkills",
                columns: table => new
                {
                    RaceSkillID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceID = table.Column<int>(nullable: false),
                    SkillID = table.Column<int>(nullable: false),
                    StartingBonus = table.Column<int>(nullable: false),
                    RateModifier = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceSkills", x => x.RaceSkillID);
                    table.ForeignKey(
                        name: "FK_RaceSkills_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceSkills_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsRessurectionPoint = table.Column<bool>(nullable: false, defaultValue: false),
                    TerrainType = table.Column<int>(nullable: false),
                    LightLevel = table.Column<int>(nullable: false),
                    RoomAreaID = table.Column<int>(nullable: false),
                    ItemIDList = table.Column<string>(nullable: true),
                    NPCIDList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomAreas_RoomAreaID",
                        column: x => x.RoomAreaID,
                        principalTable: "RoomAreas",
                        principalColumn: "RoomAreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestBackgroundReqs",
                columns: table => new
                {
                    PlayerBackgroundID = table.Column<int>(nullable: false),
                    QuestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestBackgroundReqs", x => new { x.PlayerBackgroundID, x.QuestID });
                    table.ForeignKey(
                        name: "FK_QuestBackgroundReqs_PlayerBackgrounds_PlayerBackgroundID",
                        column: x => x.PlayerBackgroundID,
                        principalTable: "PlayerBackgrounds",
                        principalColumn: "PlayerBackgroundID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestBackgroundReqs_Quests_QuestID",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestRaceReqs",
                columns: table => new
                {
                    QuestID = table.Column<int>(nullable: false),
                    RaceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestRaceReqs", x => new { x.QuestID, x.RaceID });
                    table.ForeignKey(
                        name: "FK_QuestRaceReqs_Quests_QuestID",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestRaceReqs_Races_RaceID",
                        column: x => x.RaceID,
                        principalTable: "Races",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsIdentified",
                columns: table => new
                {
                    PlayerCharacterID = table.Column<int>(nullable: false),
                    ItemID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsIdentified", x => new { x.ItemID, x.PlayerCharacterID });
                    table.ForeignKey(
                        name: "FK_ItemsIdentified_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsIdentified_Living_PlayerCharacterID",
                        column: x => x.PlayerCharacterID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerQuests",
                columns: table => new
                {
                    PlayerCharacterID = table.Column<int>(nullable: false),
                    QuestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerQuests", x => new { x.PlayerCharacterID, x.QuestID });
                    table.ForeignKey(
                        name: "FK_PlayerQuests_Living_PlayerCharacterID",
                        column: x => x.PlayerCharacterID,
                        principalTable: "Living",
                        principalColumn: "LivingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerQuests_Quests_QuestID",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomExits",
                columns: table => new
                {
                    RoomExitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExitDesc = table.Column<string>(nullable: true),
                    IsHidden = table.Column<bool>(nullable: false),
                    RoomID = table.Column<int>(nullable: true),
                    CurrentRoomID = table.Column<int>(nullable: false),
                    TargetRoomID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomExits", x => x.RoomExitID);
                    table.ForeignKey(
                        name: "FK_RoomExits_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundSkills_PlayerBackgroundID",
                table: "BackgroundSkills",
                column: "PlayerBackgroundID");

            migrationBuilder.CreateIndex(
                name: "IX_BodyLimbJoins_BodyID",
                table: "BodyLimbJoins",
                column: "BodyID");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SkillID",
                table: "Items",
                column: "SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsIdentified_PlayerCharacterID",
                table: "ItemsIdentified",
                column: "PlayerCharacterID");

            migrationBuilder.CreateIndex(
                name: "IX_Living_RaceID",
                table: "Living",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Living_PlayerBackgroundID",
                table: "Living",
                column: "PlayerBackgroundID");

            migrationBuilder.CreateIndex(
                name: "IX_Living_RoomKingdomID",
                table: "Living",
                column: "RoomKingdomID");

            migrationBuilder.CreateIndex(
                name: "IX_Living_UserID",
                table: "Living",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerQuests_QuestID",
                table: "PlayerQuests",
                column: "QuestID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestBackgroundReqs_QuestID",
                table: "QuestBackgroundReqs",
                column: "QuestID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestRaceReqs_RaceID",
                table: "QuestRaceReqs",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_SkillID",
                table: "Quests",
                column: "SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_RaceBackgrounds_PlayerBackgroundID",
                table: "RaceBackgrounds",
                column: "PlayerBackgroundID");

            migrationBuilder.CreateIndex(
                name: "IX_Races_BodyID",
                table: "Races",
                column: "BodyID");

            migrationBuilder.CreateIndex(
                name: "IX_RaceSkills_RaceID",
                table: "RaceSkills",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_RaceSkills_SkillID",
                table: "RaceSkills",
                column: "SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAreas_RoomKingdomID",
                table: "RoomAreas",
                column: "RoomKingdomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomExits_RoomID",
                table: "RoomExits",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomAreaID",
                table: "Rooms",
                column: "RoomAreaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundSkills");

            migrationBuilder.DropTable(
                name: "BodyLimbJoins");

            migrationBuilder.DropTable(
                name: "ItemsIdentified");

            migrationBuilder.DropTable(
                name: "PlayerQuests");

            migrationBuilder.DropTable(
                name: "QuestBackgroundReqs");

            migrationBuilder.DropTable(
                name: "QuestRaceReqs");

            migrationBuilder.DropTable(
                name: "RaceBackgrounds");

            migrationBuilder.DropTable(
                name: "RaceSkills");

            migrationBuilder.DropTable(
                name: "RoomExits");

            migrationBuilder.DropTable(
                name: "Limbs");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Living");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "PlayerBackgrounds");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "RoomAreas");

            migrationBuilder.DropTable(
                name: "Bodies");

            migrationBuilder.DropTable(
                name: "RoomKingdoms");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "AspNetUsers");
        }
    }
}
