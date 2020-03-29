using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Models.Quests
{
    public class PlayerQuest
    {
        public int LivingID { get; set; }
        public WanderlustRealms.Models.Living.Living Living { get; set; }
        public int QuestID { get; set; }
        public Quest Quest { get; set; }
        public bool IsComplete { get; set; }
    }
}
