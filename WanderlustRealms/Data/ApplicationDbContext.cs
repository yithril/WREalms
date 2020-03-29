using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Models;
using WanderlustRealms.Models.Backgrounds;
using WanderlustRealms.Models.GamePlay;
using WanderlustRealms.Models.Help;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Quests;
using WanderlustRealms.Models.Races;
using WanderlustRealms.Models.Role;
using WanderlustRealms.Models.Rooms;
using WanderlustRealms.Models.Shop;
using WanderlustRealms.Models.Skills;
using WanderlustRealms.Models.Training;

namespace WanderlustRealms.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Join Tables

            builder.Entity<Living>()
                .HasMany(x => x.Languages)
                .WithOne(x => x.Living)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LivingLanguage>()
                .HasKey(x => new { x.LanguageID, x.LivingID });

            builder.Entity<RaceLanguage>()
                .HasKey(x => new { x.LanguageID, x.RaceID });

            builder.Entity<RaceBackground>()
             .HasKey(x => new { x.RaceID, x.PlayerBackgroundID });

            builder.Entity<BackgroundSkill>()
                .HasKey(x => new { x.SkillID, x.PlayerBackgroundID });

            builder.Entity<PlayerQuest>()
                .HasKey(x => new { x.LivingID, x.QuestID });

            builder.Entity<BodyLimbJoin>()
                .HasKey(x => new { x.LimbID, x.BodyID });

            builder.Entity<QuestBackgroundReq>()
                .HasKey(x => new { x.PlayerBackgroundID, x.QuestID });

            builder.Entity<QuestRaceReq>()
                .HasKey(x => new { x.QuestID, x.RaceID });

            builder.Entity<ItemIdentified>()
                .HasKey(x => new { x.ItemID, x.PlayerCharacterID });

            builder.Entity<RoomExit>()
                .HasOne(x => x.CurrentRoom)
                .WithMany(y => y.RoomExits);

            builder.Entity<LimbItem>()
                .HasKey(x => new { x.ItemID, x.LimbID, x.LivingID });

            builder.Entity<PlayerSkill>()
                .HasKey(x => new { x.SkillID, x.LivingID });

            builder.Entity<GameActionJoin>()
                .HasKey(x => new { x.GameActionID, x.LivingID });

            #endregion

            #region Indexes

            builder.Entity<Room>()
                .HasIndex(b => b.RoomAreaID);

            #endregion

            #region Default Values

            builder.Entity<ApplicationUser>()
                .Property(p => p.IsPremium)
                .HasDefaultValue(false);

            builder.Entity<Room>()
                .Property(p => p.IsRessurectionPoint)
                .HasDefaultValue(false);

            builder.Entity<Item>()
                .Property(x => x.CanTake)
                .HasDefaultValue(true);

            builder.Entity<Item>()
                .Property(x => x.IsCursed)
                .HasDefaultValue(false);

            builder.Entity<Item>()
                .Property(x => x.IsMagical)
                .HasDefaultValue(false);

            builder.Entity<RoomArea>()
                .Property(x => x.IsPremium)
                .HasDefaultValue(false);

            builder.Entity<PlayerBackground>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Entity<Item>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Armor>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Armor>()
               .Property(x => x.IsAccessory)
               .HasDefaultValue(false);

            builder.Entity<Weapon>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Living>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<NPC>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Body>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Limb>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Race>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Room>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<RoomArea>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<RoomKingdom>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Skill>()
               .Property(x => x.IsActive)
               .HasDefaultValue(true);

            builder.Entity<Skill>()
               .Property(x => x.IsBackgroundSpecific)
               .HasDefaultValue(false);

            builder.Entity<PlayerBackground>()
                .Property(x => x.IsPlayable)
                .HasDefaultValue(true);

            builder.Entity<NPC>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Entity<NPC>()
                .Property(x => x.CanWander)
                .HasDefaultValue(false);

            builder.Entity<NPC>()
                .Property(x => x.IsAggressive)
                .HasDefaultValue(false);

            builder.Entity<NPC>()
                .Property(x => x.IsShopKeep)
                .HasDefaultValue(false);

            #endregion

        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<HelpItem> HelpItems { get; set; }
        public DbSet<BackgroundSkill> BackgroundSkills { get; set; }
        public DbSet<PlayerBackground> PlayerBackgrounds { get; set; }
        public DbSet<RaceBackground> RaceBackgrounds { get; set; }
        public DbSet<GameAction> GameActions { get; set; }
        public DbSet<GameActionJoin> GameActionJoins { get; set; }
        public DbSet<LimbItem> LimbItems { get; set; }
        public DbSet<Limb> Limbs { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomArea> RoomAreas { get; set; }
        public DbSet<RoomKingdom> RoomKingdoms { get; set; }
        public DbSet<RoomExit> RoomExits { get; set; }
        public DbSet<RaceSkill> RaceSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Body> Bodies { get; set; }
        public DbSet<BodyLimbJoin> BodyLimbJoins { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PlayerSkill> PlayerSkills { get; set; }

        public DbSet<ItemIdentified> ItemsIdentified { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Armor> Armor { get; set; }
        public DbSet<PlayerCharacter> PlayerCharacters { get; set; }
        public DbSet<Living> Living { get; set; }
        public DbSet<NPC> NPCs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LivingLanguage> LivingLanguages { get; set; }
        public DbSet<RaceLanguage> RaceLanguages { get; set; }

        public DbSet<TrainerPreReq> TrainerPreReqs { get; set; }
        public DbSet<TrainerSkill> TrainerSkills { get; set; }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<PlayerQuest> PlayerQuests {get; set;}
        public DbSet<QuestBackgroundReq> QuestBackgroundReqs { get; set; }
        public DbSet<QuestRaceReq> QuestRaceReqs { get; set; }

    }
}
