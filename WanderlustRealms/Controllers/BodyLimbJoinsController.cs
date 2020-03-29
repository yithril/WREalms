using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Controllers
{
    public class BodyLimbJoinsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BodyLimbJoinsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BodyLimbJoins.Include(b => b.Body).Include(b => b.Limb).OrderBy(b => b.BodyID);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyLimbJoin = await _context.BodyLimbJoins
                .Include(b => b.Body)
                .Include(b => b.Limb)
                .FirstOrDefaultAsync(m => m.LimbID == id);
            if (bodyLimbJoin == null)
            {
                return NotFound();
            }

            return View(bodyLimbJoin);
        }

        public IActionResult Create()
        {
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName");
            ViewData["LimbID"] = new SelectList(_context.Limbs, "LimbID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BodyLimbJoin bodyLimbJoin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bodyLimbJoin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName", bodyLimbJoin.BodyID);
            ViewData["LimbID"] = new SelectList(_context.Limbs, "LimbID", "Name", bodyLimbJoin.LimbID);
            return View(bodyLimbJoin);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyLimbJoin = await _context.BodyLimbJoins.FindAsync(id);
            if (bodyLimbJoin == null)
            {
                return NotFound();
            }
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName", bodyLimbJoin.BodyID);
            ViewData["LimbID"] = new SelectList(_context.Limbs, "LimbID", "Name", bodyLimbJoin.LimbID);
            return View(bodyLimbJoin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BodyLimbJoin bodyLimbJoin)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bodyLimbJoin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyLimbJoinExists(bodyLimbJoin.LimbID))
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
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName", bodyLimbJoin.BodyID);
            ViewData["LimbID"] = new SelectList(_context.Limbs, "LimbID", "Name", bodyLimbJoin.LimbID);
            return View(bodyLimbJoin);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyLimbJoin = await _context.BodyLimbJoins
                .Include(b => b.Body)
                .Include(b => b.Limb)
                .FirstOrDefaultAsync(m => m.LimbID == id);
            if (bodyLimbJoin == null)
            {
                return NotFound();
            }

            return View(bodyLimbJoin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bodyLimbJoin = await _context.BodyLimbJoins.FindAsync(id);
            _context.BodyLimbJoins.Remove(bodyLimbJoin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyLimbJoinExists(int id)
        {
            return _context.BodyLimbJoins.Any(e => e.LimbID == id);
        }
    }
}
