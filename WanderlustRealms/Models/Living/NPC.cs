using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Training;

namespace WanderlustRealms.Models.Living
{
    public class NPC : Living
    {
        public bool CanWander { get; set; }

        [Display(Name = "Does this NPC attack on sight?")]
        public bool IsAggressive { get; set; }

        [Display(Name = "Is this NPC a shopkeeper?")]
        public bool IsShopKeep { get; set; }

        [Display(Name = "Is this a unique NPC?")]
        public bool IsUnique { get; set; }

        [Display(Name = "Is this a quest giver?")]
        public bool IsQuestNPC { get; set; }

        [Display(Name = "Does this NPC train skills?")]
        public bool IsTrainer { get; set; }

        [NotMapped]
        public List<TrainerSkill> TrainerSkills { get; set; } = new List<TrainerSkill>();
    }
}
