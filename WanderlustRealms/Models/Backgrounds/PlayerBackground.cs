using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Base;

namespace WanderlustRealms.Models.Backgrounds
{
    public class PlayerBackground : EntityBase
    {
        public int PlayerBackgroundID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PicURL { get; set; }

        [Display(Name = "Playable?")]
        public bool IsPlayable { get; set; }

        public List<BackgroundSkill> BackgroundSkills { get; set; }

    }
}
