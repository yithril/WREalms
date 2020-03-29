using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Help;

namespace WanderlustRealms.Controllers
{
    public class HelpItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HelpItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult HelpComponent(string HelpTerm)
        {
            return ViewComponent("HelpComponent", new { HelpTerm = HelpTerm });
        }

        // GET: HelpItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.HelpItems.OrderBy(x => x.HelpTerm).ToListAsync());
        }

        // GET: HelpItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems
                .FirstOrDefaultAsync(m => m.HelpItemID == id);
            if (helpItem == null)
            {
                return NotFound();
            }

            return View(helpItem);
        }

        // GET: HelpItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HelpItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HelpItem helpItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(helpItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(helpItem);
        }

        // GET: HelpItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems.FindAsync(id);
            if (helpItem == null)
            {
                return NotFound();
            }
            return View(helpItem);
        }

        // POST: HelpItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HelpItem helpItem)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(helpItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HelpItemExists(helpItem.HelpItemID))
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
            return View(helpItem);
        }

        // GET: HelpItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems
                .FirstOrDefaultAsync(m => m.HelpItemID == id);
            if (helpItem == null)
            {
                return NotFound();
            }

            return View(helpItem);
        }

        // POST: HelpItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var helpItem = await _context.HelpItems.FindAsync(id);
            _context.HelpItems.Remove(helpItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HelpItemExists(int id)
        {
            return _context.HelpItems.Any(e => e.HelpItemID == id);
        }
    }
}
