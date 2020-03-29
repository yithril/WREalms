using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.Controllers
{
    public class RacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RacesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Races.OrderBy(x => x.Name).Include(r => r.Body);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.Body)
                .FirstOrDefaultAsync(m => m.RaceID == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        public IActionResult Create()
        {
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Race race)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                race.CreatedDate = DateTime.UtcNow;
                race.CreatedBy = user.Id;

                _context.Add(race);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName", race.BodyID);
            return View(race);
        }

        // GET: Races/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName", race.BodyID);
            return View(race);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Race race)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    race.ModifiedDate = DateTime.UtcNow;
                    race.ModifiedBy = user.Id;

                    _context.Update(race);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(race.RaceID))
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
            ViewData["BodyID"] = new SelectList(_context.Bodies, "BodyID", "BodyName", race.BodyID);
            return View(race);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.Body)
                .FirstOrDefaultAsync(m => m.RaceID == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var race = await _context.Races.FindAsync(id);
            _context.Races.Remove(race);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceExists(int id)
        {
            return _context.Races.Any(e => e.RaceID == id);
        }
    }
}
