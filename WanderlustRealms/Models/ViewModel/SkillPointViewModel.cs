using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.ViewModel
{
    public class SkillPointViewModel
    {
        public int RaceBonus { get; set; } = 0;
        public int BackgroundBonus { get; set; } = 0;

        public int TotalBonus { 
            get
            {
                return this.RaceBonus + this.BackgroundBonus;
            }
        }

        public string Name { get; set; }
        public int SkillID { get; set; }

        public int PlayerBackgroundID { get; set; }
    }
}
