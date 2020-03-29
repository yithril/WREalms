using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Controllers
{
    public class NPCsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NPCsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NPCs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NPCs.Include(n => n.Race);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NPCs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nPC = await _context.NPCs
                .Include(n => n.Race)
                .FirstOrDefaultAsync(m => m.LivingID == id);
            if (nPC == null)
            {
                return NotFound();
            }

            return View(nPC);
        }

        // GET: NPCs/Create
        public IActionResult Create()
        {
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NPC nPC)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nPC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", nPC.RaceID);
            return View(nPC);
        }

        // GET: NPCs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nPC = await _context.NPCs.FindAsync(id);
            if (nPC == null)
            {
                return NotFound();
            }
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", nPC.RaceID);
            return View(nPC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NPC nPC)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nPC);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NPCExists(nPC.LivingID))
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
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", nPC.RaceID);
            return View(nPC);
        }

        // GET: NPCs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nPC = await _context.NPCs
                .Include(n => n.Race)
                .FirstOrDefaultAsync(m => m.LivingID == id);
            if (nPC == null)
            {
                return NotFound();
            }

            return View(nPC);
        }

        // POST: NPCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nPC = await _context.NPCs.FindAsync(id);
            _context.NPCs.Remove(nPC);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NPCExists(int id)
        {
            return _context.NPCs.Any(e => e.LivingID == id);
        }
    }
}
