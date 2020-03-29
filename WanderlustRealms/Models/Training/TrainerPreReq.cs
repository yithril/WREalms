using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Backgrounds;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Quests;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.Training
{
    public class TrainerPreReq
    {
        public int TrainerPreReqID { get; set; }
        public int LivingID { get; set; }
        public NPC NPC { get; set; }
        public int? QuestID { get; set; }
        public Quest Quest { get; set; }
        public int? RaceID { get; set; }
        public Race Race { get; set; }

        public int? QuestPoints { get; set; }
        public int? PlayerBackgroundID { get; set; }
        public PlayerBackground PlayerBackground { get; set; }
    }
}
