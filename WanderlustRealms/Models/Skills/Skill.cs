using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Enum;

namespace WanderlustRealms.Models.Skills
{
    public class Skill : EntityBase
    {
        public int SkillID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string PicURL { get; set; }

        [Display(Name = "Specific to a background?")]

        public bool IsBackgroundSpecific { get; set; }

        [Required]
        [Display(Name = "Related Stat")]
        public Stats RelatedStat { get; set; }

        [Display(Name = "Optional Secondary Stat")]
        public Stats? SecondaryStat { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal BaseLearnRate { get; set; }

        [Display(Name = "Min. Good Alignment Level")]
        public int? MinGoodAlignment { get; set; } = 0;

        [Display(Name = "Max Good Alignment Level")] 
        public int? MaxGoodAlignment { get; set; } = 100;

        [Display(Name = "Min. Order Alignment Level")]
        public int? MinOrderAlignment { get; set; } = 0;

        [Display(Name = "Max Order Alignment Level")]
        public int? MaxOrderAlignment { get; set; } = 100;

    }
    
}
