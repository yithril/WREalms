using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Backgrounds;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Races;
using WanderlustRealms.Models.Rooms;

namespace WanderlustRealms.Models.Shop
{
    public class Shop
    {
        public int ShopID { get; set; }
        public string ShopName { get; set; }

        public int LivingID { get; set; }
        public NPC NPC { get; set; }
        public int HaggleModifier { get; set; }

        public int? PlayerBackgroundID { get; set; }

        public PlayerBackground PlayerBackground { get; set; }
        public int? RaceID { get; set; }
        public Race Race { get; set; }
        public int? RoomKingdomID { get; set; }
        public RoomKingdom RoomKingdom { get; set; }

        public List<ShopItem> ShopItems { get; set; }

        [NotMapped]
        public Dictionary<PlayerCharacter, List<Tuple<Item, int>>> HaggleMap { get; set; }
    }
}
