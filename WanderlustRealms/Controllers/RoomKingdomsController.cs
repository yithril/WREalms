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
    public class RoomKingdomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomKingdomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomKingdoms
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomKingdoms.OrderBy(x => x.Name).ToListAsync());
        }

        // GET: RoomKingdoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomKingdom = await _context.RoomKingdoms
                .FirstOrDefaultAsync(m => m.RoomKingdomID == id);
            if (roomKingdom == null)
            {
                return NotFound();
            }

            return View(roomKingdom);
        }

        // GET: RoomKingdoms/Create
        public IActionResult Create()
        {
            RoomKingdom r = new RoomKingdom();
            return View(r);
        }

        // POST: RoomKingdoms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomKingdom roomKingdom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomKingdom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomKingdom);
        }

        // GET: RoomKingdoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomKingdom = await _context.RoomKingdoms.FindAsync(id);
            if (roomKingdom == null)
            {
                return NotFound();
            }
            return View(roomKingdom);
        }

        // POST: RoomKingdoms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomKingdom roomKingdom)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomKingdom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomKingdomExists(roomKingdom.RoomKingdomID))
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
            return View(roomKingdom);
        }

        // GET: RoomKingdoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomKingdom = await _context.RoomKingdoms
                .FirstOrDefaultAsync(m => m.RoomKingdomID == id);
            if (roomKingdom == null)
            {
                return NotFound();
            }

            return View(roomKingdom);
        }

        // POST: RoomKingdoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomKingdom = await _context.RoomKingdoms.FindAsync(id);
            _context.RoomKingdoms.Remove(roomKingdom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomKingdomExists(int id)
        {
            return _context.RoomKingdoms.Any(e => e.RoomKingdomID == id);
        }
    }
}
