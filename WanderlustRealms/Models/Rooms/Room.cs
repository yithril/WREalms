using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Enum;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Models.Rooms
{
    public class Room : EntityBase
    {
        public int RoomID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsRessurectionPoint { get; set; } = false;

        [Display(Name = "Terrain")]
        public TerrainTypes TerrainType { get; set; }

        [Display(Name = "Light Level")]
        public int LightLevel { get; set; }

        public bool IsStartingRoom { get; set; }

        [Display(Name = "Room Area")]
        public int RoomAreaID { get; set; }
        public RoomArea RoomArea { get; set; }

        public string ItemIDList { get; set; } 
        public string NPCIDList { get; set; } 

        public List<RoomExit> RoomExits { get; set; } = new List<RoomExit>();

        [NotMapped]
        public List<NPC> NPCList { get; set; } = new List<NPC>();

        [NotMapped]
        public List<PlayerCharacter> PCs { get; set; } = new List<PlayerCharacter>();

        [NotMapped]
        public List<Item> ItemList { get; set; } = new List<Item>();

       
    }
}
