using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.Quests
{
    public class QuestRaceReq
    {
        public int QuestID { get; set; }
        public Quest Quest { get; set; }
        public int RaceID { get; set; }
        public Race Race { get; set; }
    }
}
