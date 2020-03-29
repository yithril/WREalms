using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Models.Backgrounds
{
    public class BackgroundSkill
    {

        [Display(Name = "Skill")]
        public int SkillID { get; set; }
        public Skill Skill { get; set; }

        public PlayerBackground Background { get; set; }

        [Display(Name = "Background")]
        public int PlayerBackgroundID { get; set; }
    }
}
