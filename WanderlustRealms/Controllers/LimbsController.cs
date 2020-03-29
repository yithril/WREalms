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
    public class LimbsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LimbsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Limbs.OrderBy(x => x.SortOrder).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limb = await _context.Limbs
                .FirstOrDefaultAsync(m => m.LimbID == id);
            if (limb == null)
            {
                return NotFound();
            }

            return View(limb);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Limb limb)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                limb.CreatedDate = DateTime.UtcNow;
                limb.CreatedBy = user.Id;

                _context.Add(limb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(limb);
        }

        // GET: Limbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limb = await _context.Limbs.FindAsync(id);
            if (limb == null)
            {
                return NotFound();
            }
            return View(limb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Limb limb)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    limb.ModifiedDate = DateTime.UtcNow;
                    limb.ModifiedBy = user.Id;

                    _context.Update(limb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LimbExists(limb.LimbID))
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
            return View(limb);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limb = await _context.Limbs
                .FirstOrDefaultAsync(m => m.LimbID == id);
            if (limb == null)
            {
                return NotFound();
            }

            return View(limb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var limb = await _context.Limbs.FindAsync(id);
            _context.Limbs.Remove(limb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LimbExists(int id)
        {
            return _context.Limbs.Any(e => e.LimbID == id);
        }
    }
}
