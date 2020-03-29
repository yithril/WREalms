using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Services
{
    public class WieldService
    {
        public bool LimbIsFull(PlayerCharacter pc, string limb)
        {
            var checkLimb = pc.Race.Body.Limbs.Where(x => x.Name.ToLower() == limb.ToLower()).FirstOrDefault();

            if (checkLimb != null)
            {
                if(checkLimb.EquippedItem != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool CheckHandednessAvailability(PlayerCharacter pc, Weapon item)
        {
            var limbs = pc.Race.Body.Limbs.ToList();

            if(pc.Race.Size > 3)
            {
                if (!item.IsOneHanded)
                {
                    item.IsOneHanded = true;
                }
            }

            var equippedItems = pc.Race.Body.Limbs.Where(x => x.EquippedItem != null).Count();
            var wieldableLimbs = pc.Race.Body.Limbs.Where(x => x.IsWieldable).Count();

            if (item.IsOneHanded)
            {
                if(wieldableLimbs - equippedItems >= 1)
                {
                    return true;
                }
            }
            else
            {
                if (wieldableLimbs - equippedItems >= 2)
                {
                    return true;
                }
            }

            return false;
        }

        public bool LimbAvailable(PlayerCharacter pc)
        {
            var limbs = pc.Race.Body.Limbs.ToList();
            var counter = 0;

            foreach(var l in limbs)
            {
                if(l.EquippedItem != null)
                {
                    counter += 1;
                }
            }

            if(counter == limbs.Count)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckRaceWield(PlayerCharacter pc, Item item)
        {
            if(pc.Race != null)
            {
                if(pc.Race.Name == "Baakthor")
                {
                    if(item.MaterialType == Models.Enum.MaterialTypes.Silver)
                    {
                        return false;
                    }
                }

                if(pc.Race.Name == "Archaen")
                {
                    if(item.MaterialType == Models.Enum.MaterialTypes.Ethereum)
                    {
                        return false;
                    }
                }

                if(pc.Race.Name == "Aethiri")
                {
                    if(item.ItemType == Models.Enum.ItemTypes.Weapon)
                    {
                        var weapon = (Weapon)item;

                        if (weapon.DamageType == Models.Enum.DamageType.water || weapon.DamageType == Models.Enum.DamageType.ice)
                        {
                            return false;
                        }
                    }

                }
            }

            return true;
        }

        public List<Limb> GetAvailableLimbs(PlayerCharacter pc, Weapon item)
        {
            var limbs = pc.Race.Body.Limbs.Where(x => x.EquippedItem == null && x.IsWieldable).ToList();
            var handcount = 1;
            var mReturn = new List<Limb>();

            //Big races can wield greatswords in one hand
            if(pc.Race.Size > 3)
            {
                item.IsOneHanded = true;
            }

            if (!item.IsOneHanded)
            {
                handcount = 2;
            }

            for(var i = 0; i < handcount; i++)
            {
                mReturn.Add(limbs[i]);
            }

            return mReturn;
        }

        public bool IsPlayerDualWielding(PlayerCharacter pc)
        {
            var limbs = pc.Race.Body.Limbs.Where(x => x.IsWieldable).ToList();

            var equipNumber = 0;

            foreach(var l in limbs)
            {
                if(l.EquippedItem != null)
                {
                    if(l.EquippedItem.ItemType == Models.Enum.ItemTypes.Weapon)
                    {
                        var currentItem = (Weapon)l.EquippedItem;
                        if (currentItem.IsOneHanded)
                        {
                            equipNumber += 1;
                        }
                    }
                }
            }

            if(equipNumber > 1)
            {
                return true;
            }

            return false;
        }

    }
}
