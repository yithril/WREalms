using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Enum;

namespace WanderlustRealms.Models.Items
{
    public class Weapon : Item
    {

        [Display(Name = "Damage Type")]
        public DamageType DamageType { get; set; }

        public int Coefficient { get; set; }

        [Display(Name = "Damage Dice")]
        public int DamageDice { get; set; }

        [Display(Name = "Damage Constant")]
        public int DamageConstant { get; set; }

        public bool IsArtifact { get; set; }

        public bool IsPrimal { get; set; }

        public bool IsOneHanded { get; set; }
    }
}
