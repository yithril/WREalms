using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.GamePlay;

namespace WanderlustRealms.Controllers
{
    public class GameActionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameActionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GameActions
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameActions.ToListAsync());
        }

        // GET: GameActions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameAction = await _context.GameActions
                .FirstOrDefaultAsync(m => m.GameActionID == id);
            if (gameAction == null)
            {
                return NotFound();
            }

            return View(gameAction);
        }

        // GET: GameActions/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameAction gameAction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameAction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameAction);
        }

        // GET: GameActions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameAction = await _context.GameActions.FindAsync(id);
            if (gameAction == null)
            {
                return NotFound();
            }
            return View(gameAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GameAction gameAction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameAction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameActionExists(gameAction.GameActionID))
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
            return View(gameAction);
        }

        // GET: GameActions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameAction = await _context.GameActions
                .FirstOrDefaultAsync(m => m.GameActionID == id);
            if (gameAction == null)
            {
                return NotFound();
            }

            return View(gameAction);
        }

        // POST: GameActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameAction = await _context.GameActions.FindAsync(id);
            _context.GameActions.Remove(gameAction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameActionExists(int id)
        {
            return _context.GameActions.Any(e => e.GameActionID == id);
        }
    }
}
