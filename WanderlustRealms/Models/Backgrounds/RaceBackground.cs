using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.Backgrounds
{
    public class RaceBackground
    {
        public Race Race { get; set; }

        [Display(Name = "Race")]
        public int RaceID { get; set; }

        [Display(Name = "Background")]
        public int PlayerBackgroundID { get; set; }
        public PlayerBackground PlayerBackground { get; set; }
    }
}
