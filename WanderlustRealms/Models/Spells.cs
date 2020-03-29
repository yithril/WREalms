using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;

namespace WanderlustRealms.Models
{
    public class Spells : EntityBase
    {
        public int SpellID { get; set; }
        public string Name { get; set; }

    }
}
