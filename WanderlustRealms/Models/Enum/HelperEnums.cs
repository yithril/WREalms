using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.Enum
{
    public enum Gender
    {
        Male,
        Female
    }
    public enum Stats
    {
        Charisma,
        Dexterity,
        Durability,
        Intellect,
        Intuition,
        Willpower
    }

    public enum TerrainTypes
    {
        Beach,
        Desert,
        [Display(Name = "Heavy Forest")]
        Heavy_Forest,
        Indoor,
        [Display(Name = "Light Forest")]
        Light_Forest,
        Mountains,
        Ocean,
        Plains,
        Swamp,
        Underwater,
        Town,
        City
    }

    public enum ItemSize
    {
        Tiny,
        Small,
        Normal,
        Large,
        Huge,
        Gargantuan
    }

    public enum ItemTypes
    {
        Grey,
        Weapon,
        Armor,
        Book,
        Key,
        Quest
    }

    public enum MaterialTypes
    {
        Iron,
        Steel,
        Wood,
        Bronze,
        Marble,
        Obsidian,
        Stone,
        Ethereum,
        [Display(Name = "Ghost Ore")]
        Ghost_Ore,
        Miasma,
        Other,
        Leather,
        Silver,
        Gold,
        Glass,
        Mercury,
        Hide
    }

    public enum DamageType
    {
        slashing,
        crushing,
        piercing,
        fire,
        ice,
        water,
        photonic,
        necromantic,
        order,
        chaos,
        holy
    }

    public enum GoodAlignmentChoices
    {
        Good,
        Neutral,
        Evil
    }

    public enum OrderAlignmentChoices
    {
        Ordered,
        Neutral,
        Chaotic
    }
}
