using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;

namespace WanderlustRealms.Models.Rooms
{
    public class RoomKingdom : EntityBase
    {
        public int RoomKingdomID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]

        public string Description { get; set; }
        public List<RoomArea> RoomAreas { get; set; }
    }
}
