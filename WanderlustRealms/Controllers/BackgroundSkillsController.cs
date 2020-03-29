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
    public class BackgroundSkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BackgroundSkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BackgroundSkills.Include(b => b.Background).Include(b => b.Skill).OrderBy(x => x.Background.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var backgroundSkill = await _context.BackgroundSkills
                .Include(b => b.Background)
                .Include(b => b.Skill)
                .FirstOrDefaultAsync(m => m.SkillID == id);
            if (backgroundSkill == null)
            {
                return NotFound();
            }

            return View(backgroundSkill);
        }

        public IActionResult Create()
        {
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name");
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(X => X.Name).Select(x => new { x.SkillID, Name = x.Name + "-" + x.RelatedStat.ToString() }), "SkillID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BackgroundSkill backgroundSkill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(backgroundSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name", backgroundSkill.PlayerBackgroundID);
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(X => X.RelatedStat).Select(x => new { x.SkillID, Name = x.Name + "-" + x.RelatedStat.ToString() }), "SkillID", "Name");
            return View(backgroundSkill);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var backgroundSkill = await _context.BackgroundSkills.FindAsync(id);
            if (backgroundSkill == null)
            {
                return NotFound();
            }
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name", backgroundSkill.PlayerBackgroundID);
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(X => X.RelatedStat).Select(x => new { x.SkillID, Name = x.Name + "-" + x.RelatedStat.ToString() }), "SkillID", "Name");
            return View(backgroundSkill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BackgroundSkill backgroundSkill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(backgroundSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BackgroundSkillExists(backgroundSkill.SkillID))
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
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "Name", backgroundSkill.PlayerBackgroundID);
            ViewData["SkillID"] = new SelectList(_context.Skills.OrderBy(X => X.RelatedStat).Select(x => new { x.SkillID, Name = x.Name + "-" + x.RelatedStat.ToString() }), "SkillID", "Name");
            return View(backgroundSkill);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var backgroundSkill = await _context.BackgroundSkills
                .Include(b => b.Background)
                .Include(b => b.Skill)
                .FirstOrDefaultAsync(m => m.SkillID == id);
            if (backgroundSkill == null)
            {
                return NotFound();
            }

            return View(backgroundSkill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var backgroundSkill = await _context.BackgroundSkills.FindAsync(id);
            _context.BackgroundSkills.Remove(backgroundSkill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BackgroundSkillExists(int id)
        {
            return _context.BackgroundSkills.Any(e => e.SkillID == id);
        }
    }
}
