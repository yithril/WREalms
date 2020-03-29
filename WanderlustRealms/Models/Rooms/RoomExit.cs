using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.Rooms
{
    public class RoomExit
    {
        public int RoomExitID { get; set; }
        public string ExitDesc { get; set; }

        public bool IsHidden { get; set; }

        [ForeignKey("RoomID")]
        public virtual Room CurrentRoom { get; set; }

        public int CurrentRoomID { get; set; }

        public int TargetRoomID { get; set; }

        [NotMapped]
        public List<int> RevealedPlayerIDList { get; set; }
    }
}
