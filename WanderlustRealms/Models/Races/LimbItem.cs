using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Models.Races
{
    public class LimbItem
    {
        public int LivingID { get; set; }
        public int LimbID { get; set; }
        public Limb Limb { get; set; }
        public int ItemID { get; set; }
        public Item Item { get; set; }
    }
}
