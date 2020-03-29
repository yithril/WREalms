using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Backgrounds;

namespace WanderlustRealms.Models.Quests
{
    public class QuestBackgroundReq
    {
        public int PlayerBackgroundID { get; set; }
        public PlayerBackground PlayerBackground { get; set; }

        public int QuestID { get; set; }
        public Quest Quest { get; set; }
    }
}
