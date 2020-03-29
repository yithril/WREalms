using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Enum;
using WanderlustRealms.Models.Races;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Models.Items
{
    public class Armor : Item
    {
        public int ArmorPoints { get; set; }
        public bool IsPrimal { get; set; }
        public string LimbIDList { get; set; }

        [Display(Name = "Boost Skill by how much?")]
        public int SkillBoost { get; set; }

        [Display(Name = "Boost stat by how much?")]
        public int StatBoost { get; set; }

        public Stats StatToBoost { get; set; }

        [Display(Name = "Skill to Boost")]
        public int? SkillID { get; set; }

        public Skill Skill { get; set; }

        public bool IsAccessory { get; set; }

        public ArmorClass ArmorClass { get; set; }

        [NotMapped]
        public List<Limb> Limbs { get; set; } = new List<Limb>();
    }

    public enum ArmorClass
    {
        Light,
        Medium,
        Heavy
    }
}
