using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Items;

namespace WanderlustRealms.Models.Races
{
    public class Limb : EntityBase
    {
        public int LimbID { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Is it vital?")]
        public bool IsVital { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Can it wield weapons?")]
        public bool IsWieldable { get; set; }

        [Display(Name = "Does it enable flight?")]
        public bool CanFly { get; set; }

        [Display(Name = "Is it enhanced unarmed?")]
        public bool IsEnhancedUnarmed { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [NotMapped]
        public Item EquippedItem { get; set; }

        [NotMapped]
        public int ItemTag { get; set; }
    }
}
