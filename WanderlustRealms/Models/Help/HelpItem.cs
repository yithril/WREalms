using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.Help
{
    public class HelpItem
    {
        public int HelpItemID { get; set; }

        [Required]
        [Display(Name = "Help Term")]
        public string HelpTerm { get; set; }

        [Required]
        [Display(Name = "Help Description")]
        public string HelpDescription { get; set; }

        [Display(Name = "Display URL?")]
        public string URL { get; set; }
    }
}
