using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Backgrounds;
using WanderlustRealms.Models.Enum;
using WanderlustRealms.Models.Races;
using WanderlustRealms.Models.Rooms;

namespace WanderlustRealms.Models.ViewModel
{
    public class PCViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Select your race:")]
        [Required]
        public int RaceID { get; set; }
        public List<Race> RaceList { get; set; }

        [Display(Name = "Select your character's background:")]
        [Required]
        public int PlayerBackgroundID { get; set; }
        public List<PlayerBackground> BackgroundList { get; set; }

        public int RoomKingdomID { get; set; }
        public RoomKingdom RoomKingdom { get; set; }

        public Gender Gender { get; set; }

        public int SetInt { get; set; }
        public int SetDur { get; set; }
        public int SetDex { get; set; }
        public int SetCha { get; set; }
        public int SetIntuit { get; set; }
        public int SetWill { get; set; }

        public int MinInt { get; set; }
        public int MaxInt { get; set; }
        public int MinDur { get; set; }
        public int MaxDur { get; set; }
        public int MinDex { get; set; }
        public int MaxDex { get; set; }
        public int MinCha { get; set; }
        public int MaxCha { get; set; }
        public int MinIntuit { get; set; }
        public int MaxIntuit { get; set; }
        public int MinWill { get; set; }
        public int MaxWill { get; set; }

        public List<SkillSubObject> skills { get; set; }

        public GoodAlignmentChoices GoodAlignChoice { get; set; }
        public OrderAlignmentChoices OrderAlignChoice { get; set; }
        int GoodAlignment { get; set; }
        int OrderAlignment { get; set; }
    }

    public class SkillSubObject
    {
        public int total { get; set; }
        public int skillID { get; set; }
    }


}
