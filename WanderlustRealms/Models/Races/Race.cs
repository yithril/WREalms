using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Backgrounds;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Models.Races
{
    public class Race : EntityBase
    {
        [Key]
        public int RaceID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Minimum Light Level")]
        [Required]
        public int MinLightLevel { get; set; }

        [Required]
        public int Size { get; set; }

        [Display(Name = "Premium Race?")]
        public bool IsPremium { get; set; }

        [Display(Name = "Player Race?")]
        public bool IsPlayable { get; set; }

        [Required]
        [Display(Name = "Starting Stat Points")]
        public int StatPoints { get; set; }

        [Required]
        [Display(Name = "Starting Skill Points")]
        public int SkillPoints { get; set; }

        public string PicURL { get; set; }

        public List<RaceBackground> RaceBackgrounds { get; set; } = new List<RaceBackground>();

        public List<RaceSkill> RaceSkills { get; set; } = new List<RaceSkill>();

        public int BodyID { get; set; }
        public Body Body { get; set; }

        #region Base Statistics

        [Required]
        [Display(Name = "Minimum Intellect Score")]

        public int MinInt { get; set; }

        [Required]
        [Display(Name = "Maximum Intellect Score")]
        public int MaxInt { get; set; }

        [Required]
        [Display(Name = "Maximum Durability Score")]
        public int MaxDur { get; set; }

        [Required]
        [Display(Name = "Minimum Durability Score")]
        public int MinDur { get; set; }

        [Required]
        [Display(Name = "Minimum Intuition Score")]
        public int MinIntuit { get; set; }

        [Required]
        [Display(Name = "Maximum Intuition Score")]
        public int MaxIntuit { get; set; }

        [Required]
        [Display(Name = "Minimum Dexterity Score")]
        public int MinDex { get; set; }

        [Required]
        [Display(Name = "Maximum Dexterity Score")]
        public int MaxDex { get; set; }

        [Required]
        [Display(Name = "Minimum Willpower Score")]
        public int MinWill { get; set; }

        [Required]
        [Display(Name = "Maximum Willpower Score")]
        public int MaxWill { get; set; }

        [Required]
        [Display(Name = "Minimum Charisma Score")]
        public int MinCha { get; set; }

        [Required]
        [Display(Name = "Maximum Charisma Score")]
        public int MaxCha { get; set; }


        #endregion


    }
}
