using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.ViewModel
{
    public class ArmorLimb
    {
        public int ItemID { get; set; }
        public Item Item { get; set; }
        public int LimbID { get; set; }
        public Limb Limb { get; set; }
    }
}
