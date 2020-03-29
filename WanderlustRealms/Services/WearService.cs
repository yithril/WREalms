using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Services
{
    public class WearService
    {
        public bool RaceCanWear(PlayerCharacter pc, Armor a)
        {
            if(pc.Race.Name == "Baakthor")
            {
                if(a.MaterialType == Models.Enum.MaterialTypes.Silver)
                {
                    return false;
                }
            }

            if(pc.Race.Name == "Aethiri")
            {
                if (a.Name.ToLower().Contains("ice"))
                {
                    return false;
                }
            }

            if(pc.Race.Name == "Archaen")
            {
                if (a.MaterialType == Models.Enum.MaterialTypes.Ethereum)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
