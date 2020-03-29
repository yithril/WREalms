using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Models.Skills
{
    public class PlayerSkill
    {
        public int SkillID { get; set; }
        public Skill Skill { get; set; }
        public int LivingID { get; set; }
        public WanderlustRealms.Models.Living.Living Living { get; set; }

        public double Level { get; set; }
    }
}
