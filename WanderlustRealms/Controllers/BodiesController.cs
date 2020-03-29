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
    public class BodiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BodiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Bodies.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var body = await _context.Bodies
                .FirstOrDefaultAsync(m => m.BodyID == id);
            if (body == null)
            {
                return NotFound();
            }

            return View(body);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Body body)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                body.CreatedDate = DateTime.UtcNow;
                body.CreatedBy = user.Id;

                _context.Add(body);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(body);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var body = await _context.Bodies.FindAsync(id);
            if (body == null)
            {
                return NotFound();
            }
            return View(body);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Body body)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    body.ModifiedDate = DateTime.UtcNow;
                    body.ModifiedBy = user.Id;

                    _context.Update(body);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyExists(body.BodyID))
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
            return View(body);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var body = await _context.Bodies
                .FirstOrDefaultAsync(m => m.BodyID == id);
            if (body == null)
            {
                return NotFound();
            }

            return View(body);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var body = await _context.Bodies.FindAsync(id);
            _context.Bodies.Remove(body);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyExists(int id)
        {
            return _context.Bodies.Any(e => e.BodyID == id);
        }
    }
}
