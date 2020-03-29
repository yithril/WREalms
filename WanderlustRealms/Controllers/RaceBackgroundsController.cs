using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Backgrounds;

namespace WanderlustRealms.Controllers
{
    public class RaceBackgroundsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceBackgroundsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RaceBackgrounds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RaceBackgrounds.Include(r => r.PlayerBackground).Include(r => r.Race).OrderBy(x => x.Race.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RaceBackgrounds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceBackground = await _context.RaceBackgrounds
                .Include(r => r.PlayerBackground)
                .Include(r => r.Race)
                .FirstOrDefaultAsync(m => m.RaceID == id);
            if (raceBackground == null)
            {
                return NotFound();
            }

            return View(raceBackground);
        }

        // GET: RaceBackgrounds/Create
        public IActionResult Create()
        {
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name");
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaceBackground raceBackground)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceBackground);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name", raceBackground.PlayerBackgroundID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", raceBackground.RaceID);
            return View(raceBackground);
        }

        // GET: RaceBackgrounds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceBackground = await _context.RaceBackgrounds.FindAsync(id);
            if (raceBackground == null)
            {
                return NotFound();
            }
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name", raceBackground.PlayerBackgroundID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", raceBackground.RaceID);
            return View(raceBackground);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RaceBackground raceBackground)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceBackground);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceBackgroundExists(raceBackground.RaceID))
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
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name", raceBackground.PlayerBackgroundID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", raceBackground.RaceID);
            return View(raceBackground);
        }

        // GET: RaceBackgrounds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceBackground = await _context.RaceBackgrounds
                .Include(r => r.PlayerBackground)
                .Include(r => r.Race)
                .FirstOrDefaultAsync(m => m.RaceID == id);
            if (raceBackground == null)
            {
                return NotFound();
            }

            return View(raceBackground);
        }

        // POST: RaceBackgrounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceBackground = await _context.RaceBackgrounds.FindAsync(id);
            _context.RaceBackgrounds.Remove(raceBackground);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceBackgroundExists(int id)
        {
            return _context.RaceBackgrounds.Any(e => e.RaceID == id);
        }
    }
}
