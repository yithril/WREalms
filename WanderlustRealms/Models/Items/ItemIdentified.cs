using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Models.Items
{
    public class ItemIdentified
    {
        public int PlayerCharacterID { get; set; }
        public PlayerCharacter PlayerCharacter { get; set; }

        public int ItemID { get; set; }
        public Item Item { get; set; }
    }
}
