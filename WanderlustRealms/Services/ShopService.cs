using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Shop;

namespace WanderlustRealms.Services
{
    public class ShopService
    {
        private readonly ApplicationDbContext _context;

        public ShopService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Item> ListShopItemsByNPC(int LivingID, PlayerCharacter pc)
        {
            var shop = _context.Shops.Where(x => x.LivingID == LivingID).FirstOrDefault();

            var items = _context.ShopItems.Where(x => x.ShopID == shop.ShopID).Include(x => x.Item).ToList();
            var mReturn = new List<Item>();

            foreach(var shopItem in items)
            {
                if (!shopItem.CanHaggle)
                {
                    mReturn.Add(shopItem.Item);
                    continue;
                }

                if (!shop.HaggleMap.ContainsKey(pc))
                {
                    mReturn.Add(shopItem.Item);
                    continue;
                }

                var haggleCost = shop.HaggleMap[pc].Where(x => x.Item1.ItemID == shopItem.Item.ItemID).Select(x => x.Item2).FirstOrDefault();

                if(haggleCost != 0)
                {
                    shopItem.Item.SalesCost = haggleCost;
                    mReturn.Add(shopItem.Item);
                }
                else
                {
                    mReturn.Add(shopItem.Item);
                }
            }

            return mReturn;
        }

        public Tuple<int, string> Haggle(PlayerCharacter pc, Shop shop, Item item, bool IsBuying)
        {
            var haggle = pc.PlayerSkills.Where(x => x.Skill.Name == "Haggle").FirstOrDefault();
            var shopkeep = _context.NPCs.Where(X => X.LivingID == shop.LivingID).Select(X => X.Name).FirstOrDefault();
            var cost = item.Cost;

            Random rnd = new Random();
            var playerScore = rnd.Next(1, 51);
            var npcRoll = rnd.Next(1, 51);

            var npcScore = npcRoll + shop.HaggleModifier;

            if(haggle != null)
            {
                playerScore += (int)(haggle.Level + (pc.Charisma / 2));
            }
            else
            {
                playerScore += (int)pc.Charisma / 2;
            }

            var dif = playerScore - npcScore;

            var per = GetDifferencePercentage(dif);

            if (IsBuying)
            {
                cost -= (int)(cost * per);
            }
            else
            {
                cost += (int)(cost * per);
            }

            if(cost < 0)
            {
                cost = 1;
            }

            return new Tuple<int, string>(item1: cost, item2: BargainingSuccess(dif, IsBuying, shopkeep));
        }

        public string BargainingSuccess(int dif, bool IsBuying, string shopKeep)
        {
            var mReturn = "";

            if(dif < 26 && dif > -26)
            {
                mReturn = "You feel you were not able to get " + shopKeep + " to budge much on price.";
            }
            else if (dif > -51 && dif < -26)
            {
                mReturn = "You feel like " + shopKeep + " got the better deal.";
            }
            else if (dif > -76 && dif < -50)
            {
                mReturn = "You definitely didn't get the better end of the deal.";
            }
            else if (dif > -101 && dif < -75)
            {
                mReturn = shopKeep + " outmaneuvers you in negotiations.";
            }
            else if (dif > -126 && dif < -100)
            {
                mReturn = shopKeep + " outsmarts you in your negotiations.";
            }
            else if (dif > -151 && dif < -125)
            {
                mReturn = "You felt you had a good deal initially, but now you're not so sure.";
            }
            else if (dif > -176 && dif < -150)
            {
                mReturn = shopKeep + " completely had you over a barrel.";
            }
            else if (dif < -175)
            {
                mReturn = shopKeep + " took you to the cleaners.";
            }
            else if(dif > 25 && dif < 51)
            {
                mReturn = "You were able to negotiate a decent deal with " + shopKeep + ".";
            }
            else if(dif > 50 && dif < 76)
            {
                mReturn = "You feel like you are getting a good deal.";
            }
            else if(dif > 75 && dif < 101)
            {
                mReturn = "Your negotations with " + shopKeep + "go really well.";
            }
            else if (dif > 100 && dif < 126)
            {
                mReturn = "You did very well in your negotiations.";
            }
            else if (dif > 125 && dif < 151)
            {
                mReturn = "You feel you had the definite advantage bargaining with " + shopKeep + ".";
            }
            else if (dif > 150 && dif < 176)
            {
                mReturn = "You get an excellent price.";
            }
            else if (dif > 175 && dif < 201)
            {
                mReturn = "You get away with highway robbery in your negotiations with " + shopKeep + ".";
            }
            else if (dif > 200)
            {
                mReturn = "You masterfully bargain with " + shopKeep + " and get an AMAZING deal.";
            }

            return mReturn;
        }

        public void AddToShopHaggleMap(Shop shop, PlayerCharacter pc, Item item, int cost)
        {
            if (!shop.HaggleMap.ContainsKey(pc))
            {
                shop.HaggleMap.Add(pc, new List<Tuple<Item, int>>() { new Tuple<Item, int>(item, cost) });
            }
            else
            {
                shop.HaggleMap[pc].Add(new Tuple<Item, int>(item, cost));
            }
        }

        private double GetDifferencePercentage(int dif)
        {
            return 0.05 * (dif / 25);
        }

    }
}
