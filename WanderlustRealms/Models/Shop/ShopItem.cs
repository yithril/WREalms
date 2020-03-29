using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Items;

namespace WanderlustRealms.Models.Shop
{
    public class ShopItem
    {
        public int ShopItemID { get; set; }
        public int ItemID { get; set; }
        public Item Item { get; set; }
        public int ShopID { get; set; }
        public Shop Shop { get; set; }

        public bool CanHaggle = false;
    }
}
