using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Rooms;

namespace WanderlustRealms.Controllers
{
    public class RoomExitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomExitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomExits
        public async Task<IActionResult> Index(int RoomID)
        {
            ViewBag.RoomID = RoomID;
            ViewBag.RoomAreaID = _context.Rooms.Where(x => x.RoomID == RoomID).Select(x => x.RoomAreaID).FirstOrDefault();

            return View(await _context.RoomExits.Where(X => X.CurrentRoomID == RoomID).ToListAsync());
        }

        // GET: RoomExits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomExit = await _context.RoomExits
                .FirstOrDefaultAsync(m => m.RoomExitID == id);
            if (roomExit == null)
            {
                return NotFound();
            }

            return View(roomExit);
        }

        // GET: RoomExits/Create
        public IActionResult Create(int RoomID)
        {
            RoomExit e = new RoomExit();
            e.CurrentRoomID = RoomID;

            var RoomAreaID = _context.Rooms.Where(x => x.RoomID == RoomID).Select(x => x.RoomAreaID).FirstOrDefault();

            ViewData["TargetRoomList"] = new SelectList(_context.Rooms.Where(x => x.RoomAreaID == RoomAreaID && x.RoomID != RoomID), "RoomID", "Title");
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomExit roomExit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomExit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { RoomID = roomExit.CurrentRoomID });
            }
            return View(roomExit);
        }

        // GET: RoomExits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomExit = await _context.RoomExits.FindAsync(id);
            if (roomExit == null)
            {
                return NotFound();
            }
            return View(roomExit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomExit roomExit)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomExit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExitExists(roomExit.RoomExitID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { RoomID = roomExit.CurrentRoomID });
            }
            return View(roomExit);
        }

        // GET: RoomExits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomExit = await _context.RoomExits
                .FirstOrDefaultAsync(m => m.RoomExitID == id);
            if (roomExit == null)
            {
                return NotFound();
            }

            return View(roomExit);
        }

        // POST: RoomExits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomExit = await _context.RoomExits.FindAsync(id);
            _context.RoomExits.Remove(roomExit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExitExists(int id)
        {
            return _context.RoomExits.Any(e => e.RoomExitID == id);
        }
    }
}
