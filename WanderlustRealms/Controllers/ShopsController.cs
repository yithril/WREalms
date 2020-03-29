using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Shop;

namespace WanderlustRealms.Controllers
{
    public class ShopsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shops
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Shops.Include(s => s.PlayerBackground).Include(s => s.Race).Include(s => s.RoomKingdom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.PlayerBackground)
                .Include(s => s.Race)
                .Include(s => s.RoomKingdom)
                .FirstOrDefaultAsync(m => m.ShopID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            ViewData["LivingID"] = new SelectList(_context.NPCs.Where(x => x.IsShopKeep), "LivingID", "Name");
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name");
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name");
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LivingID"] = new SelectList(_context.NPCs.Where(x => x.IsShopKeep), "LivingID", "Name");
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name");
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name");
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name");
            return View(shop);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            ViewData["LivingID"] = new SelectList(_context.NPCs.Where(x => x.IsShopKeep), "LivingID", "Name");
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "PlayerBackgroundID", shop.PlayerBackgroundID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Description", shop.RaceID);
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Description", shop.RoomKingdomID);
            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Shop shop)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.ShopID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LivingID"] = new SelectList(_context.NPCs.Where(x => x.IsShopKeep), "LivingID", "Name");
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name");
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name");
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name");
            return View(shop);
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.PlayerBackground)
                .Include(s => s.Race)
                .Include(s => s.RoomKingdom)
                .FirstOrDefaultAsync(m => m.ShopID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.ShopID == id);
        }
    }
}
