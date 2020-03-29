using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.ViewModel;

namespace WanderlustRealms.Controllers
{
    public class ArmorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArmorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ArmorLimbs(int ItemID)
        {
            ViewBag.ItemID = ItemID;
            ViewBag.ArmorName = _context.Items.Where(x => x.ItemID == ItemID).Select(x => x.Name).FirstOrDefault();

            var item = _context.Items.Find(ItemID);
            var armor = (Armor)item;
            var mReturn = new List<ArmorLimb>();

            if (!string.IsNullOrEmpty(armor.LimbIDList))
            {
                var limbIDList = armor.LimbIDList.Split(",");

                foreach(string s in limbIDList)
                {
                    ArmorLimb l = new ArmorLimb();
                    l.ItemID = armor.ItemID;
                    l.Item = armor;
                    l.LimbID = Int32.Parse(s);
                    l.Limb = _context.Limbs.Find(l.LimbID);
                    mReturn.Add(l);
                }
            }

            return View(mReturn);
        }

        public async Task<IActionResult> AddArmorLimbs(int ItemID)
        {
            ViewBag.ItemID = ItemID;

            ViewData["LimbList"] = new SelectList(_context.Limbs.OrderBy(x => x.Name), "LimbID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddArmorLimbs(ArmorLimb limb)
        {
            var armor = _context.Armor.Find(limb.ItemID);

            if (!string.IsNullOrEmpty(armor.LimbIDList))
            {
                armor.LimbIDList += ("," + limb.LimbID);
            }
            else
            {
                armor.LimbIDList = limb.LimbID.ToString();
            }

            _context.Update(armor);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArmorLimbs", new { limb.ItemID });
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Armor.Include(a => a.Skill);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armor = await _context.Armor
                .Include(a => a.Skill)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (armor == null)
            {
                return NotFound();
            }

            return View(armor);
        }

        public IActionResult Create()
        {
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Armor armor)
        {
            if (ModelState.IsValid)
            {
                armor.ItemType = Models.Enum.ItemTypes.Armor;
                _context.Add(armor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name", armor.SkillID);
            return View(armor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armor = await _context.Armor.FindAsync(id);
            if (armor == null)
            {
                return NotFound();
            }
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name", armor.SkillID);
            return View(armor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Armor armor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(armor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArmorExists(armor.ItemID))
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
            ViewData["SkillID"] = new SelectList(_context.Skills, "SkillID", "Name", armor.SkillID);
            return View(armor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armor = await _context.Armor
                .Include(a => a.Skill)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (armor == null)
            {
                return NotFound();
            }

            return View(armor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var armor = await _context.Armor.FindAsync(id);
            _context.Armor.Remove(armor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArmorExists(int id)
        {
            return _context.Armor.Any(e => e.ItemID == id);
        }
    }
}
