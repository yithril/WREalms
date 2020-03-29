using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Controllers
{
    public class RaceSkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceSkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RaceSkills
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RaceSkills.Include(r => r.Race).Include(r => r.Skill);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RaceSkills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceSkill = await _context.RaceSkills
                .Include(r => r.Race)
                .Include(r => r.Skill)
                .FirstOrDefaultAsync(m => m.RaceSkillID == id);
            if (raceSkill == null)
            {
                return NotFound();
            }

            return View(raceSkill);
        }

        // GET: RaceSkills/Create
        public IActionResult Create()
        {
            ViewData["RaceID"] = new SelectList(_context.Races.OrderBy(x => x.Name), "RaceID", "Name");
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(x => x.Name), "SkillID", "Name");

            RaceSkill r = new RaceSkill();
            r.RateModifier = 1;
            return View(r);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaceSkill raceSkill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RaceID"] = new SelectList(_context.Races.OrderBy(x => x.Name), "RaceID", "Name", raceSkill.RaceID);
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(x => x.Name), "SkillID", "Name", raceSkill.SkillID);
            return View(raceSkill);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceSkill = await _context.RaceSkills.FindAsync(id);
            if (raceSkill == null)
            {
                return NotFound();
            }
            ViewData["RaceID"] = new SelectList(_context.Races.OrderBy(x => x.Name), "RaceID", "Name", raceSkill.RaceID);
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(x => x.Name), "SkillID", "Name", raceSkill.SkillID);
            return View(raceSkill);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RaceSkill raceSkill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceSkillExists(raceSkill.RaceSkillID))
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
            ViewData["RaceID"] = new SelectList(_context.Races.OrderBy(x => x.Name), "RaceID", "Name", raceSkill.RaceID);
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(x => x.Name), "SkillID", "Name", raceSkill.SkillID);
            return View(raceSkill);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceSkill = await _context.RaceSkills
                .Include(r => r.Race)
                .Include(r => r.Skill)
                .FirstOrDefaultAsync(m => m.RaceSkillID == id);
            if (raceSkill == null)
            {
                return NotFound();
            }

            return View(raceSkill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceSkill = await _context.RaceSkills.FindAsync(id);
            _context.RaceSkills.Remove(raceSkill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceSkillExists(int id)
        {
            return _context.RaceSkills.Any(e => e.RaceSkillID == id);
        }
    }
}
