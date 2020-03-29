using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.Living
{
    public class RaceLanguage
    {
        public int RaceID { get; set; }
        public Race Race { get; set; }
        public int LanguageID { get; set; }
        public Language Language { get; set; }

        [Display(Name = "Starting Level: 0 is nothing 100 is fluent")]
        public int StartingLevel { get; set; }
        public bool IsMain { get; set; }
    }
}
