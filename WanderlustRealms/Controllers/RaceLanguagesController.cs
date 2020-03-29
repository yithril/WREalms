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
    public class RaceLanguagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceLanguagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RaceLanguages
        public async Task<IActionResult> Index(int RaceID)
        {
            ViewBag.RaceID = RaceID;
            ViewBag.Name = _context.Races.Where(x => x.RaceID == RaceID).Select(x => x.Name).FirstOrDefault();

            var applicationDbContext = _context.RaceLanguages.Where(x => x.RaceID == RaceID).Include(r => r.Language).Include(r => r.Race);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RaceLanguages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceLanguage = await _context.RaceLanguages
                .Include(r => r.Language)
                .Include(r => r.Race)
                .FirstOrDefaultAsync(m => m.LanguageID == id);
            if (raceLanguage == null)
            {
                return NotFound();
            }

            return View(raceLanguage);
        }

        // GET: RaceLanguages/Create
        public IActionResult Create(int RaceID)
        {
            ViewData["LanguageID"] = new SelectList(_context.Languages, "LanguageID", "Name");
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name");

            RaceLanguage r = new RaceLanguage();
            r.RaceID = RaceID;
            r.Race = _context.Races.Find(RaceID);

            return View(r);
        }

        // POST: RaceLanguages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaceLanguage raceLanguage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceLanguage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "RaceLanguages", new { RaceID = raceLanguage.RaceID });
            }
            ViewData["LanguageID"] = new SelectList(_context.Languages, "LanguageID", "Name", raceLanguage.LanguageID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", raceLanguage.RaceID);
            return View(raceLanguage);
        }

        // GET: RaceLanguages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceLanguage = await _context.RaceLanguages.FindAsync(id);
            if (raceLanguage == null)
            {
                return NotFound();
            }
            ViewData["LanguageID"] = new SelectList(_context.Languages, "LanguageID", "Name", raceLanguage.LanguageID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Name", raceLanguage.RaceID);
            return View(raceLanguage);
        }

        // POST: RaceLanguages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RaceLanguage raceLanguage)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceLanguage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceLanguageExists(raceLanguage.LanguageID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "RaceLanguages", new { RaceID = raceLanguage.RaceID });
            }
            ViewData["LanguageID"] = new SelectList(_context.Languages, "LanguageID", "Name", raceLanguage.LanguageID);
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Description", raceLanguage.RaceID);
            return View(raceLanguage);
        }

        // GET: RaceLanguages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceLanguage = await _context.RaceLanguages
                .Include(r => r.Language)
                .Include(r => r.Race)
                .FirstOrDefaultAsync(m => m.LanguageID == id);
            if (raceLanguage == null)
            {
                return NotFound();
            }

            return View(raceLanguage);
        }

        // POST: RaceLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceLanguage = await _context.RaceLanguages.FindAsync(id);
            _context.RaceLanguages.Remove(raceLanguage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceLanguageExists(int id)
        {
            return _context.RaceLanguages.Any(e => e.LanguageID == id);
        }
    }
}
