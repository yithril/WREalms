using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Models.Quests
{
    public class Quest : EntityBase
    {
        public int QuestID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Quest Points")]
        public int QuestPoints { get; set; }

        [Display(Name = "Minimum Level")]
        public int MinLevel { get; set; }

        [Display(Name = "Maximum Level")]
        public int MaxLevel { get; set; }

        [Display(Name = "Gold Reward")]
        public int GoldReward { get; set; }

        [Display(Name = "XP Reward")]
        public int XPReward { get; set; }

        [Display(Name = "Skill Point Reward")]
        public int? SkillID { get; set; }
        public Skill Skill { get; set; }
        public int SkillBonusPoints { get; set; }

        public int LivingID { get; set; }
        public NPC NPC { get; set; }

        [Display(Name = "NPC starting speech")]
        public string QuestStartDialogue { get; set; }

        public List<QuestBackgroundReq> PlayerBackgroundReq { get; set; }
        public List<QuestRaceReq> RaceRequirement { get; set; }
    }
}
