using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;

namespace WanderlustRealms.Models.Rooms
{
    public class RoomArea : EntityBase
    {
        public int RoomAreaID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Min Level")]
        public int MinLevel { get; set; }

        [Display(Name = "Max Level")]
        public int? MaxLevel { get; set; }

        [Display(Name = "Premium?")]
        public bool IsPremium { get; set; }

        [Display(Name = "Kingdom")]
        public int RoomKingdomID { get; set; }
        public RoomKingdom RoomKingdom { get; set; }

        public List<Room> Rooms { get; set; }
    }
}
