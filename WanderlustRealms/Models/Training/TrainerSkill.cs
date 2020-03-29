using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Models.Training
{
    public class TrainerSkill
    {
        public int TrainerSkillID { get; set; }
        public int LivingID { get; set; }
        public NPC NPC { get; set; }
        public int SkillID { get; set; }
        public Skill Skill { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
    }
}
