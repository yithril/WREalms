using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Items;

namespace WanderlustRealms.Models.Quests
{
    public class QuestNode
    {
        public int QuestNodeID { get; set; }
        public int QuestID { get; set; }
        public Quest Quest { get; set; }
        public int ParentID { get; set; }

        public int? ItemID { get; set; }
        public Item Item { get; set; }

    }

    public enum QuestType
    {
        Fetch,
        Deliver,
        Kill_Unique,
        Kill_Group,
    }
}
