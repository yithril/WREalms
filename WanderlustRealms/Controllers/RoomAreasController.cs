using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Rooms;

namespace WanderlustRealms.Controllers
{
    public class RoomAreasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomAreasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomAreas
        public async Task<IActionResult> Index(int RoomKingdomID)
        {
            ViewBag.RoomKingdomID = RoomKingdomID;
            ViewBag.KingdomName = _context.RoomKingdoms.Where(x => x.RoomKingdomID == RoomKingdomID).Select(x => x.Name).FirstOrDefault();

            var applicationDbContext = _context.RoomAreas.Where(x => x.RoomKingdomID == RoomKingdomID).Include(r => r.RoomKingdom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RoomAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomArea = await _context.RoomAreas
                .Include(r => r.RoomKingdom)
                .FirstOrDefaultAsync(m => m.RoomAreaID == id);
            if (roomArea == null)
            {
                return NotFound();
            }

            return View(roomArea);
        }

        // GET: RoomAreas/Create
        public IActionResult Create(int RoomKingdomID)
        {
            RoomArea ra = new RoomArea();
            ra.RoomKingdomID = RoomKingdomID;

            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name");
            return View(ra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomArea roomArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { RoomKingdomID = roomArea.RoomKingdomID});
            }
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name", roomArea.RoomKingdomID);
            return View(roomArea);
        }

        // GET: RoomAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomArea = await _context.RoomAreas.FindAsync(id);
            if (roomArea == null)
            {
                return NotFound();
            }
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name", roomArea.RoomKingdomID);
            return View(roomArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomArea roomArea)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomAreaExists(roomArea.RoomAreaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { RoomKingdomID = roomArea.RoomKingdomID });
            }
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "Name", roomArea.RoomKingdomID);
            return View(roomArea);
        }

        // GET: RoomAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomArea = await _context.RoomAreas
                .Include(r => r.RoomKingdom)
                .FirstOrDefaultAsync(m => m.RoomAreaID == id);
            if (roomArea == null)
            {
                return NotFound();
            }

            return View(roomArea);
        }

        // POST: RoomAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomArea = await _context.RoomAreas.FindAsync(id);
            _context.RoomAreas.Remove(roomArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomAreaExists(int id)
        {
            return _context.RoomAreas.Any(e => e.RoomAreaID == id);
        }
    }
}
