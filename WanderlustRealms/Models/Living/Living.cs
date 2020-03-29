using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Interfaces;
using WanderlustRealms.Models.Base;
using WanderlustRealms.Models.GamePlay;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Models.Living
{
    public class Living : EntityBase, IDescription
    {
        public int LivingID { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Short Description")]
        [Required]
        public string ShortDesc { get; set; }

        [Required]
        public string Description { get; set; }

        public string Keywords { get; set; }

        [Display(Name = "Race")]
        public int RaceID { get; set; }
        public Race Race { get; set; }

        public int Gold { get; set; }

        public string ItemIDList { get; set; }

        [NotMapped]
        public List<Item> InventoryList { get; set; } = new List<Item>();

        public int GoodAlignment { get; set; }
        public int OrderAlignment { get; set; }

        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }

        public int Durability { get; set; }
        public int Dexterity { get; set; }
        public int Charisma { get; set; }
        public int Intellect { get; set; }
        public int Willpower { get; set; }
        public int Intuition { get; set; }

        public List<GameActionJoin> GameActionJoins { get; set; }
        public List<LivingLanguage> Languages { get; set; }
        public int MainLanguageID { get; set; }

        [NotMapped]
        public List<GameAction> Actions { get; set; } = new List<GameAction>();

        public string GetLongDescription()
        {
            return this.Description;
        }

        public string GetShortDescription()
        {
            return this.ShortDesc;
        }

        public string GetName()
        {
            return this.Name;
        }

        public string GetGoodAlignment()
        {
            if(this.GoodAlignment < 34)
            {
                return "Evil";
            }
            else if(this.GoodAlignment > 33 && this.GoodAlignment < 67)
            {
                return "Neutral";
            }
            else
            {
                return "Good";
            }
        }

        public string GetOrderAlignment()
        {
            if (this.OrderAlignment < 34)
            {
                return "Chaotic";
            }
            else if (this.OrderAlignment > 33 && this.OrderAlignment < 67)
            {
                return "Neutral";
            }
            else
            {
                return "Ordered";
            }
        }
    }
}
