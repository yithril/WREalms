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
using WanderlustRealms.Models.Backgrounds;

namespace WanderlustRealms.Controllers
{
    public class PlayerBackgroundsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerBackgroundsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.PlayerBackgrounds.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerBackground = await _context.PlayerBackgrounds
                .FirstOrDefaultAsync(m => m.PlayerBackgroundID == id);
            if (playerBackground == null)
            {
                return NotFound();
            }

            return View(playerBackground);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerBackground playerBackground)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                playerBackground.CreatedDate = DateTime.UtcNow;
                playerBackground.CreatedBy = user.Id;

                _context.Add(playerBackground);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playerBackground);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerBackground = await _context.PlayerBackgrounds.FindAsync(id);
            if (playerBackground == null)
            {
                return NotFound();
            }
            return View(playerBackground);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerBackground playerBackground)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    playerBackground.ModifiedDate = DateTime.UtcNow;
                    playerBackground.ModifiedBy = user.Id;

                    _context.Update(playerBackground);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerBackgroundExists(playerBackground.PlayerBackgroundID))
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
            return View(playerBackground);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerBackground = await _context.PlayerBackgrounds
                .FirstOrDefaultAsync(m => m.PlayerBackgroundID == id);
            if (playerBackground == null)
            {
                return NotFound();
            }

            return View(playerBackground);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerBackground = await _context.PlayerBackgrounds.FindAsync(id);
            _context.PlayerBackgrounds.Remove(playerBackground);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerBackgroundExists(int id)
        {
            return _context.PlayerBackgrounds.Any(e => e.PlayerBackgroundID == id);
        }
    }
}
