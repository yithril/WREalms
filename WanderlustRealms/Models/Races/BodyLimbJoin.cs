using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.Races
{
    public class BodyLimbJoin
    {
        [Display(Name = "Body Type")]
        public int BodyID { get; set; }
        public Body Body { get; set; }
        public int LimbID { get; set; }
        public Limb Limb { get; set; }
    }
}
