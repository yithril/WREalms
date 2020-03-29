using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;

namespace WanderlustRealms.Models.Races
{
    public class Body : EntityBase
    {
        public int BodyID { get; set; }

        [Display(Name = "Body Name")]
        [Required]
        public string BodyName { get; set; }
        public List<BodyLimbJoin> LimbJoin { get; set; }

        [NotMapped]
        public List<Limb> Limbs { get; set; }
    }
}
