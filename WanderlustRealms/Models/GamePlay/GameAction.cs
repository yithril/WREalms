using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.GamePlay
{
    public class GameAction
    {
        public int GameActionID { get; set; }

        [Required]
        public string Verb { get; set; }

        [Display(Name = "How many words in the command?")]
        [Required]
        public int CommandLength { get; set; } = 1;

        public bool IsStandard { get; set; }

        [Display(Name = "What other commands could activate this?")]
        public string AlternateKeywords { get; set; }

        [Required]
        public string Function { get; set; }
    }

    public class GameActionJoin
    {
        public int GameActionID { get; set; }
        public GameAction GameAction { get; set; }
        public int LivingID { get; set; }
        public WanderlustRealms.Models.Living.Living Living { get; set; }
    }
}
