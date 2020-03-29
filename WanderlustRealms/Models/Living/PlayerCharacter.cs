using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Backgrounds;
using WanderlustRealms.Models.Enum;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Quests;
using WanderlustRealms.Models.Races;
using WanderlustRealms.Models.Rooms;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Models.Living
{
    public class PlayerCharacter : Living
    {
        public ApplicationUser User { get; set; }
        public string UserID { get; set; }
        public int PlayerBackgroundID { get; set; }
        public PlayerBackground Background { get; set; }

        public Gender Gender { get; set; }

        public int XP { get; set; }

        public int XPToSpend { get; set; }
        public int Level { get; set; }

        [Display(Name = "HomeLand")]
        public int RoomKingdomID { get; set; }
        public RoomKingdom RoomKingdom { get; set; }

        public int LastRoomID { get; set; }

        public bool IsHaggleOn { get; set; }

        public List<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();

        public List<ItemIdentified> IdentifiedItems { get; set; } = new List<ItemIdentified>();

        public List<PlayerSkill> PlayerSkills { get; set; } = new List<PlayerSkill>();
        
    }
}
