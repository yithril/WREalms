using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Interfaces;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.Enum;

namespace WanderlustRealms.Models.Items
{
    public class Item : EntityBase, IDescription
    {
        public int ItemID { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Short Description")]
        [Required]
        public string ShortDesc { get; set; }

        [Display(Name = "Long Description")]
        [Required]
        public string LongDesc { get; set; }

        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "Item Identified Description")]
        public string HowItWorksDesc { get; set; }

        public ItemSize Size { get; set; }

        public int Cost { get; set; }
        public bool CanTake { get; set; }
        public int Weight { get; set; }
        public MaterialTypes MaterialType { get; set; }
        public bool IsMagical { get; set; }
        public bool IsCursed { get; set; }

        public ItemTypes ItemType { get; set; }

        [NotMapped]
        public int SalesCost { get; set; }
        

        public string GetName()
        {
            return this.Name;
        }
        public string GetLongDescription()
        {
            return this.LongDesc;
        }

        public string GetShortDescription()
        {
            return this.ShortDesc;
        }
    }


}
