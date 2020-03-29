using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Training;

namespace WanderlustRealms.Controllers
{
    public class TrainerSkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerSkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainerSkills
        public async Task<IActionResult> Index(int LivingID)
        {
            ViewBag.LivingID = LivingID;
            ViewBag.Name = _context.NPCs.Where(x => x.LivingID == LivingID).Select(x => x.Name).FirstOrDefault();

            var applicationDbContext = _context.TrainerSkills.Where(x => x.LivingID == LivingID).Include(t => t.Skill);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TrainerSkills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerSkill = await _context.TrainerSkills
                .Include(t => t.Skill)
                .FirstOrDefaultAsync(m => m.TrainerSkillID == id);
            if (trainerSkill == null)
            {
                return NotFound();
            }

            return View(trainerSkill);
        }

        // GET: TrainerSkills/Create
        public IActionResult Create(int LivingID)
        {
            TrainerSkill s = new TrainerSkill();
            s.LivingID = LivingID;
            s.NPC = _context.NPCs.Find(LivingID);

            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name");
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerSkill trainerSkill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainerSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TrainerSkills", new { LivingID = trainerSkill.LivingID });
            }
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name", trainerSkill.SkillID);
            return View(trainerSkill);
        }

        // GET: TrainerSkills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerSkill = await _context.TrainerSkills.FindAsync(id);
            if (trainerSkill == null)
            {
                return NotFound();
            }
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name", trainerSkill.SkillID);
            return View(trainerSkill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrainerSkill trainerSkill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainerSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerSkillExists(trainerSkill.TrainerSkillID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "TrainerSkills", new { LivingID = trainerSkill.LivingID });
            }
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name", trainerSkill.SkillID);
            return View(trainerSkill);
        }

        // GET: TrainerSkills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerSkill = await _context.TrainerSkills
                .Include(t => t.Skill)
                .FirstOrDefaultAsync(m => m.TrainerSkillID == id);
            if (trainerSkill == null)
            {
                return NotFound();
            }

            return View(trainerSkill);
        }

        // POST: TrainerSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainerSkill = await _context.TrainerSkills.FindAsync(id);
            _context.TrainerSkills.Remove(trainerSkill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerSkillExists(int id)
        {
            return _context.TrainerSkills.Any(e => e.TrainerSkillID == id);
        }
    }
}
