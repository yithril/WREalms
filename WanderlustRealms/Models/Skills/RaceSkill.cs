using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.Skills
{
    public class RaceSkill
    {
        public int RaceSkillID { get; set; }

        [Display(Name = "Select Race")]
        public int RaceID { get; set; }
        public Race Race { get; set; }

        [Display(Name = "Select Skill")]
        public int SkillID { get; set; }
        public Skill Skill { get; set; }

        [Required]
        [Display(Name = "Starting Bonus")]
        public int StartingBonus { get; set; }

        [Required]
        [Display(Name = "Negative numbers are a penalty")]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal RateModifier { get; set; }
    }
}
