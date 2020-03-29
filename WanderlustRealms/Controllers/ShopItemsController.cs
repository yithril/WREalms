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
    public class ShopItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShopItems
        public async Task<IActionResult> Index(int ShopID)
        {
            var applicationDbContext = _context.ShopItems.Where(x => x.ShopID == ShopID).Include(s => s.Item).Include(s => s.Shop);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShopItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems
                .Include(s => s.Item)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.ShopItemID == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // GET: ShopItems/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name");
            ViewData["ShopID"] = new SelectList(_context.Shops, "ShopID", "ShopName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ShopItems", new { ShopID = shopItem.ShopID });
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name", shopItem.ItemID);
            ViewData["ShopID"] = new SelectList(_context.Shops, "ShopID", "ShopName", shopItem.ShopID);
            return View(shopItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name", shopItem.ItemID);
            ViewData["ShopID"] = new SelectList(_context.Shops, "ShopID", "ShopName", shopItem.ShopID);
            return View(shopItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShopItem shopItem)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopItemExists(shopItem.ShopItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "ShopItems", new { ShopID = shopItem.ShopID });
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name", shopItem.ItemID);
            ViewData["ShopID"] = new SelectList(_context.Shops, "ShopID", "ShopName", shopItem.ShopID);
            return View(shopItem);
        }

        // GET: ShopItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems
                .Include(s => s.Item)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.ShopItemID == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // POST: ShopItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopItem = await _context.ShopItems.FindAsync(id);
            _context.ShopItems.Remove(shopItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopItemExists(int id)
        {
            return _context.ShopItems.Any(e => e.ShopItemID == id);
        }
    }
}
